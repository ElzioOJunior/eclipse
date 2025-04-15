using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using EclipseApi.Application.Commands;
using EclipseApi.Application.Dtos;
using EclipseApi.Domain.Entities;
using EclipseApi.Domain.Interface;
using MediatR;

public class AddCommentHandler : IRequestHandler<AddCommentCommand, CommentDto>
{
    private readonly IRepository<Comment> _commentRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<EclipseApi.Domain.Entities.Task> _taskRepository;
    private readonly IRepository<TaskHistory> _taskHistoryRepository;
    private readonly IMapper _mapper;

    public AddCommentHandler(IRepository<Comment> commentRepository, IRepository<TaskHistory> taskHistoryRepository, IRepository<User> userRepository, IRepository<EclipseApi.Domain.Entities.Task> taskRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _taskHistoryRepository = taskHistoryRepository;
        _userRepository = userRepository;
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<CommentDto> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        await ValidateUserExistsAsync(request.UserId);

        await ValidateTaskExistsAsync(request.TaskId);

        var comment = CreateComment(request);

        await SaveCommentAsync(comment);

        var history = CreateHistoryEntry(request, comment.TaskId);

        await SaveHistoryAsync(history);

        return MapToDto(comment);
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
    private async System.Threading.Tasks.Task ValidateTaskExistsAsync(Guid taskId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
        {
            throw new CustomException(
                type: "InvalidData",
                message: "Task not found",
                detail: $"The task with ID {taskId} does not exist in the system."
            );
        }
    }

    private Comment CreateComment(AddCommentCommand request)
    {
        var comment = _mapper.Map<Comment>(request);
        comment.Id = Guid.NewGuid();
        comment.CreatedAt = DateTime.UtcNow;
        return comment;
    }

    private async System.Threading.Tasks.Task SaveCommentAsync(Comment comment)
    {
        await _commentRepository.AddAsync(comment);
    }

    private TaskHistory CreateHistoryEntry(AddCommentCommand request, Guid taskId)
    {
        return new TaskHistory
        {
            Id = Guid.NewGuid(),
            TaskId = taskId,
            ChangedField = "Comment",
            PreviousValue = string.Empty,
            NewValue = request.Content,
            ChangedDate = DateTime.UtcNow,
            ChangedBy = request.UserId
        };
    }

    private async System.Threading.Tasks.Task SaveHistoryAsync(TaskHistory history)
    {
        await _taskHistoryRepository.AddAsync(history);
    }

    private CommentDto MapToDto(Comment comment)
    {
        return _mapper.Map<CommentDto>(comment);
    }

}
