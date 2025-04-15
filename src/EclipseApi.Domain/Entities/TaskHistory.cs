using System;

namespace EclipseApi.Domain.Entities
{
    public class TaskHistory: BaseEntity
    {
        public Guid TaskId { get; set; }
        public Task Task { get; set; }
        public string ChangedField { get; set; }
        public string PreviousValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ChangedDate { get; set; }
        public Guid ChangedBy { get; set; } 
    }

}
