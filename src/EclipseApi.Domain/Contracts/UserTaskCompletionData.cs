
namespace EclipseApi.Domain.Contracts
{
    public class UserTaskCompletionData
    {
        public Guid UserId { get; set; }
        public int CompletedTasksCount { get; set; }
        public double AverageCompletedTasks { get; set; } 
    }
}
