using Microsoft.EntityFrameworkCore;
using SDU.Contexts.Auth.Domain.Entities;
using SDU.Contexts.Auth.Domain.IRepositories;
using SDU.Contexts.Auth.Infrastructure.Persistance;

namespace SDU.Contexts.Auth.Infrastructure.Repositories;

public class EntityRepository : IEntityRepository
{
    private readonly  AuthDbContext _context;
    public EntityRepository(AuthDbContext context)
    {
        _context = context;
    }
    
    public async Task IncrementBytesAsync(string entityId, long bytes)
    {
        await _context.Entities
            .Where(e => e.Id == entityId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(e => e.TotalUsedStorage, e => e.TotalUsedStorage + bytes)
            );
    }

    public async Task IncrementFileCounterAsync(string entityId)
    {
        await _context.Entities
            .Where(e => e.Id == entityId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(e => e.FileAmount,  e => e.FileAmount + 1)
            );
    }
    
    public async Task RegisterFileAsync(string entityId, long bytes)
    {
        await _context.Entities
            .Where(e => e.Id == entityId)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(e => e.FileAmount, e => e.FileAmount + 1)
                    .SetProperty(e => e.TotalUsedStorage, e => e.TotalUsedStorage + bytes)
            );
    }

    public async Task<List<Entity>> ListEntitesAsync(string? name, string? email)
    {
        IQueryable<Entity> query = _context.Entities.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(e => e.Name == name);

        
        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(e => e.UserName.ToLower() == email.ToLower());
        
        return await query.ToListAsync();
    }
}