using EclipseApi.Domain.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EclipseApi.Domain.Entities.Task>> GetTasksByProjectIdAsync(Guid projectId)
    {
        return await _context.Tasks.AsNoTracking()
            .Include(t => t.Histories).AsNoTracking()
            .Include(t => t.Comments).AsNoTracking()
            .Where(t => t.ProjectId == projectId) 
            .ToListAsync();
    }
}
