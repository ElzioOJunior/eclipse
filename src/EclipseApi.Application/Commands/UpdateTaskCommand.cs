using EclipseApi.Application.Dtos;
using MediatR;

namespace EclipseApi.Application.Commands
{
    public class UpdateTaskCommand : IRequest<TaskDto>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid UpdatedByUserId { get; set; }
        public Domain.Enum.TaskStatus Status { get; set; }

        public Guid UserId { get; set; }

        public UpdateTaskCommand(Guid id, string title, string description, Guid updatedByUserId, Domain.Enum.TaskStatus status, Guid userId)
        {
            Id = id;
            Title = title;
            Description = description;
            UpdatedByUserId = updatedByUserId;
            Status = status;
            UserId = userId;
        }
    }

}
