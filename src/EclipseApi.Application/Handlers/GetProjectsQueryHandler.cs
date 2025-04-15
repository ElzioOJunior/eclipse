using AutoMapper;
using EclipseApi.Application.Dtos;
using EclipseApi.Application.Queries;
using EclipseApi.Domain.Interface;
using MediatR;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, IEnumerable<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public GetProjectsQueryHandler(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetProjectsByUserIdAsync(request.UserId);
        return _mapper.Map<IEnumerable<ProjectDto>>(projects);
    }
}
