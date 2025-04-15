using System;

namespace EclipseApi.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public Guid TaskId { get; set; }
        public Task Task { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; } 
    }
}
