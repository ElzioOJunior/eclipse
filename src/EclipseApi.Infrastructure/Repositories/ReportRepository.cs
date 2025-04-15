using EclipseApi.Domain.Contracts;
using EclipseApi.Domain.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _context;

    public ReportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserTaskCompletionData>> GetAverageCompletedTasksPerUserAsync(Guid userId)
    {
        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

        return await _context.Tasks.AsNoTracking()
            .Where(t => t.UserId == userId && t.Status == EclipseApi.Domain.Enum.TaskStatus.Concluida && t.DueDate >= thirtyDaysAgo)
            .GroupBy(t => t.UserId)
            .Select(g => new UserTaskCompletionData
            {
                UserId = g.Key,
                CompletedTasksCount = g.Count(),
                AverageCompletedTasks = Math.Round(g.Count() / 30.0, 2)
            })
            .ToListAsync();
    }


}

