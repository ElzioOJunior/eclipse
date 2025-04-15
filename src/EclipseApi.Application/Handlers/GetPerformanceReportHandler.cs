using AutoMapper;
using EclipseApi.Application.Dtos;
using EclipseApi.Application.Queries;
using EclipseApi.Domain.Interface;
using MediatR;

public class GetPerformanceReportHandler : IRequestHandler<GetPerformanceReportQuery, IEnumerable<PerformanceReportDto>>
{
    private readonly IReportRepository _reportRepository;
    private readonly IMapper _mapper;

    public GetPerformanceReportHandler(IReportRepository reportRepository, IMapper mapper)
    {
        _reportRepository = reportRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PerformanceReportDto>> Handle(GetPerformanceReportQuery request, CancellationToken cancellationToken)
    {
        var reportData = await _reportRepository.GetAverageCompletedTasksPerUserAsync(request.UserId);

        return _mapper.Map<IEnumerable<PerformanceReportDto>>(reportData);
    }
}
