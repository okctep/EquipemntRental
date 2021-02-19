using Domain;
using MediatR;
using Newtonsoft.Json;
using Presentation.Commands;
using System.Threading;
using System.Threading.Tasks;


namespace Presentation.Handlers
{
    public class GetInvoiceHandler : IRequestHandler<GetInvoice, Invoice>
    {
        private readonly RpcClient _rpcClient;

        public GetInvoiceHandler(RpcClient rpcClient)
        {
            _rpcClient = rpcClient;
        }

        public async Task<Invoice> Handle(GetInvoice request, CancellationToken cancellationToken)
        {
            var message = JsonConvert.SerializeObject(request.ConstructionEquipment);
            var response = await _rpcClient.CallAsync(message);

            return JsonConvert.DeserializeObject<Invoice>(response);
        }
    }
}
