using EclipseApi.Domain.Enum;
using System;

namespace EclipseApi.Domain.Entities
{
    public class Task : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public EclipseApi.Domain.Enum.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; } 

        public Project Project { get; set; }
        public User User { get; set; } 
        public ICollection<Comment> Comments { get; set; }
        public ICollection<TaskHistory> Histories { get; set; }

        public Task Clone()
        {
            return new Task
            {
                Id = this.Id,
                Title = this.Title,
                Description = this.Description,
                DueDate = this.DueDate,
                Status = this.Status,
                Priority = this.Priority,
                ProjectId = this.ProjectId,
                UserId = this.UserId
            };
        }
    }



}
