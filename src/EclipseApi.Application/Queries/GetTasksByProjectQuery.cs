using EclipseApi.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseApi.Application.Queries
{
    public class GetTasksByProjectQuery : IRequest<IEnumerable<TaskDto>>
    {
        public Guid ProjectId { get; set; }
    }
}

