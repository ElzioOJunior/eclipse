using System;

namespace EclipseApi.Api.Helpers
{
    public class UpdateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid UpdatedByUserId { get; set; }
        public Domain.Enum.TaskStatus Status { get; set; }
        public Guid UserId { get; set; }
    }
}
