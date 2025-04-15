using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using EclipseApi.Application.Commands;
using EclipseApi.Application.Dtos;
using EclipseApi.Domain.Entities;
using MediatR;

public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly IRepository<EclipseApi.Domain.Entities.Task> _taskRepository;
    private readonly IRepository<Project> _projectRepository;
    private readonly IRepository<User> _userRepository; // Adicionado para validar o usuário
    private readonly IMapper _mapper;

    public CreateTaskHandler(
        IRepository<EclipseApi.Domain.Entities.Task> taskRepository,
        IRepository<Project> projectRepository,
        IRepository<User> userRepository,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var project = await ValidateProjectAsync(request.ProjectId);
        var user = await ValidateUserAsync(request.UserId);

        ValidateTaskLimit(project);

        var task = CreateTask(request, user);

        await SaveTaskAsync(task);

        return MapToDto(task);
    }

    private async Task<Project> ValidateProjectAsync(Guid projectId)
    {
        var project = await _projectRepository.GetByIdWithIncludesAsync(projectId, project => project.Tasks);
        if (project == null)
        {
            throw new CustomException(
                type: "InvalidData",
                message: "Project not found",
                detail: $"The project with ID {projectId} was not found in the system."
            );
        }
        return project;
    }

    private async Task<User> ValidateUserAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new CustomException(
                type: "InvalidData",
                message: "User not found",
                detail: $"The user with ID {userId} was not found in the system."
            );
        }
        return user;
    }

    private void ValidateTaskLimit(Project project)
    {
        if (project.Tasks != null && project.Tasks.Count >= 20)
        {
            throw new CustomException(
                type: "InvalidData",
                message: "Task limit reached",
                detail: $"Task limit reached for Project ID {project.Id}. The limit is 20 tasks per project."
            );
        }
    }

    private EclipseApi.Domain.Entities.Task CreateTask(CreateTaskCommand request, User user)
    {
        var task = _mapper.Map<EclipseApi.Domain.Entities.Task>(request);
        task.Id = Guid.NewGuid();
        task.Status = EclipseApi.Domain.Enum.TaskStatus.Pendente;
        task.UserId = user.Id; 
        return task;
    }

    private async System.Threading.Tasks.Task SaveTaskAsync(EclipseApi.Domain.Entities.Task task)
    {
        await _taskRepository.AddAsync(task);
    }

    private TaskDto MapToDto(EclipseApi.Domain.Entities.Task task)
    {
        return _mapper.Map<TaskDto>(task);
    }
}
