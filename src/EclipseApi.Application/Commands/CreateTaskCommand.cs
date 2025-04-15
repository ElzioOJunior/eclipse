using EclipseApi.Application.Dtos;
using EclipseApi.Domain.Enum;
using MediatR;
namespace EclipseApi.Application.Commands
{
    public class CreateTaskCommand : IRequest<TaskDto>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
    }

}
