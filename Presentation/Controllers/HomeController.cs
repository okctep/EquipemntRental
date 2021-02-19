using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Presentation.Commands;
using Presentation.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediator;
        private readonly IOptions<List<EquipmentViewModel>> _contructionEquipemnt;

        public HomeController(ILogger<HomeController> logger, IMediator mediator, IOptions<List<EquipmentViewModel>> contructionEquipemnt)
        {
            _logger = logger;
            _mediator = mediator;
            _contructionEquipemnt = contructionEquipemnt;
        }

        public IActionResult Index()
        {
            return View(_contructionEquipemnt.Value);
        }

        [HttpPost]
        public async Task<string> GetInvoice(List<ConstructionEquipment> constructionEquipemnt) {

            var resp = await _mediator.Send(new GetInvoice()
            {
                ConstructionEquipment = constructionEquipemnt
            });
            return JsonConvert.SerializeObject(resp);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
