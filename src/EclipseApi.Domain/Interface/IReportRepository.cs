using EclipseApi.Domain.Contracts;

namespace EclipseApi.Domain.Interface
{
    public interface IReportRepository
    {
        Task<IEnumerable<UserTaskCompletionData>> GetAverageCompletedTasksPerUserAsync(Guid userId);
    }

}
