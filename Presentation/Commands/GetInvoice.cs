using Domain;
using MediatR;
using System.Collections.Generic;

namespace Presentation.Commands
{
    public class GetInvoice : IRequest<Invoice>
    {
        public List<ConstructionEquipment> ConstructionEquipment { get; set; }
    }
}
