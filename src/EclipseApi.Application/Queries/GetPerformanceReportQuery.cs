using EclipseApi.Application.Dtos;
using MediatR;

namespace EclipseApi.Application.Queries
{
    public class GetPerformanceReportQuery : IRequest<IEnumerable<PerformanceReportDto>>
    {
        public Guid UserId { get; set; }
    }

}
