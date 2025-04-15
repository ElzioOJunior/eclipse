using EclipseApi.Domain.Entities;
using EclipseApi.Domain.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(Guid userId)
    {
        return await _context.Projects.AsNoTracking()
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }
}
