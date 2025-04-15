using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using EclipseApi.Application.Commands;
using EclipseApi.Application.Dtos;
using EclipseApi.Application.Helpers;
using EclipseApi.Domain.Entities;
using MediatR;

public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
{
    private readonly IRepository<EclipseApi.Domain.Entities.Task> _taskRepository;
    private readonly IRepository<TaskHistory> _taskHistoryRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public UpdateTaskHandler(IRepository<EclipseApi.Domain.Entities.Task> taskRepository, IRepository<TaskHistory> taskHistoryRepository, IRepository<User> userRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _taskHistoryRepository = taskHistoryRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        await ValidateUserExistsAsync(request.UserId);

        var task = await GetTaskByIdAsync(request.Id);

        var originalTask = CloneTask(task);

        UpdateTaskEntity(request, task);

        var changes = IdentifyChanges(originalTask, task);

        await SaveUpdatedTaskAsync(task);

        await RegisterChangesInHistoryAsync(changes, task.Id, request.UpdatedByUserId);

        return MapToDto(task);
    }

    private async System.Threading.Tasks.Task ValidateUserExistsAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new CustomException(
                type: "InvalidData",
                message: "User not found",
                detail: $"The user with ID {userId} does not exist in the system."
            );
        }
    }


    private async Task<EclipseApi.Domain.Entities.Task> GetTaskByIdAsync(Guid taskId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
        {
            throw new CustomException(
                type: "InvalidData",
                message: "Task not found",
                detail: $"The task with ID {taskId} was not found in the system."
            );
        }
        return task;
    }

    private EclipseApi.Domain.Entities.Task CloneTask(EclipseApi.Domain.Entities.Task task)
    {
        return new EclipseApi.Domain.Entities.Task
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status,
            Priority = task.Priority,
            ProjectId = task.ProjectId
        };
    }
    private void UpdateTaskEntity(UpdateTaskCommand request, EclipseApi.Domain.Entities.Task task)
    {
        _mapper.Map(request, task);
    }


    private List<ChangeLog> IdentifyChanges(EclipseApi.Domain.Entities.Task originalTask, EclipseApi.Domain.Entities.Task updatedTask)
    {
        var changes = new List<ChangeLog>();

        var properties = typeof(EclipseApi.Domain.Entities.Task).GetProperties();
        foreach (var property in properties)
        {
            var originalValue = property.GetValue(originalTask)?.ToString();
            var updatedValue = property.GetValue(updatedTask)?.ToString();

            if (originalValue != updatedValue)
            {
                changes.Add(new ChangeLog
                {
                    FieldName = property.Name,
                    PreviousValue = originalValue,
                    NewValue = updatedValue
                });
            }
        }

        return changes;
    }

    private async System.Threading.Tasks.Task RegisterChangesInHistoryAsync(List<ChangeLog> changes, Guid taskId, Guid userId)
    {
        foreach (var change in changes)
        {
            var history = new TaskHistory
            {
                Id = Guid.NewGuid(),
                TaskId = taskId,
                ChangedField = change.FieldName,
                PreviousValue = change.PreviousValue,
                NewValue = change.NewValue,
                ChangedDate = DateTime.UtcNow,
                ChangedBy = userId
            };

            await _taskHistoryRepository.AddAsync(history);
        }
    }
    private async System.Threading.Tasks.Task SaveUpdatedTaskAsync(EclipseApi.Domain.Entities.Task task)
    {
        await _taskRepository.UpdateAsync(task);
    }

    private TaskDto MapToDto(EclipseApi.Domain.Entities.Task task)
    {
        return _mapper.Map<TaskDto>(task);
    }

}
