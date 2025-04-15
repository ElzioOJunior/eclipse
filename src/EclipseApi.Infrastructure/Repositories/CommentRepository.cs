using EclipseApi.Domain.Entities;
using EclipseApi.Domain.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class CommentRepository : ICommentRepository
{
    private readonly AppDbContext _context;

    public CommentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comment>> GetCommentsByTaskIdAsync(Guid taskId)
    {
        return await _context.Comments.AsNoTracking()
            .Where(c => c.TaskId == taskId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
}
