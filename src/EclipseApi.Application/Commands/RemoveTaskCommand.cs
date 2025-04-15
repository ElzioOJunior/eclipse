using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Application.Commands
{
    public class RemoveTaskCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
