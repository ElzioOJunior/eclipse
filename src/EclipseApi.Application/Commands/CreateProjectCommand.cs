using EclipseApi.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Application.Commands
{
    public class CreateProjectCommand : IRequest<ProjectDto>
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
    }

}
