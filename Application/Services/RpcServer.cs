using Application.Configuration;
using Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    class RpcServer
    {
        private readonly ILogger _logger;
        private readonly IOptions<RpcServerConfiguration> _options;
        private readonly RentalPriceCalculationService _rentalPriceCalculationService;

        public RpcServer(ILogger logger, IOptions<RpcServerConfiguration> options, RentalPriceCalculationService service)
        {
            _options = options;
            _logger = logger;
            _rentalPriceCalculationService = service;
        }

        public void Start()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _options.Value.RabbitHost,
                    UserName = _options.Value.RabbitUserName,
                    Password = _options.Value.RabbitPassword,
                    Port = _options.Value.RabbitPort,
                    DispatchConsumersAsync = true
                };
                var connection = factory.CreateConnection();
                using (var channel = connection.CreateModel())
                {

                    channel.QueueDeclare(queue: "rpc_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    channel.BasicQos(0, 1, false);
                    var consumer = new AsyncEventingBasicConsumer(channel);
                    channel.BasicConsume(queue: "rpc_queue", autoAck: false, consumer: consumer);
                    Console.WriteLine(" [x] Awaiting RPC requests");
                    consumer.Received += async (model, ea) =>
                    {
                        string response = null;

                        var body = ea.Body.ToArray();
                        var props = ea.BasicProperties;
                        var replyProps = channel.CreateBasicProperties();
                        replyProps.CorrelationId = props.CorrelationId;

                        try
                        {
                            var message = Encoding.UTF8.GetString(body);
                            var rentedEquipment = JsonConvert.DeserializeObject<List<ConstructionEquipment>>(message);
                            var invoice = _rentalPriceCalculationService.GenerateInvoice(rentedEquipment);
                            Console.WriteLine("Incoming message: ", message);
                            response = JsonConvert.SerializeObject(invoice);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(" [.] " + e.Message);
                            response = "";
                        }
                        finally
                        {
                            var responseBytes = Encoding.UTF8.GetBytes(response);
                            channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                            await Task.Yield();
                        }
                    };

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(-1, ex, "RabbitMQClient init fail");
            }
        }

    }
}
