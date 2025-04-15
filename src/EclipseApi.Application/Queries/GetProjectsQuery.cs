using EclipseApi.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Application.Queries
{
    public class GetProjectsQuery : IRequest<IEnumerable<ProjectDto>>
    {
        public Guid UserId { get; set; }
    }

}
