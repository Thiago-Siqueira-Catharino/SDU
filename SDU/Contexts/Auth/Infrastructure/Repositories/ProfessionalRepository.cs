using Microsoft.EntityFrameworkCore;
using SDU.Contexts.Auth.Domain.Entities;
using SDU.Contexts.Auth.Domain.IRepositories;
using SDU.Contexts.Auth.Infrastructure.Persistance;

namespace SDU.Contexts.Auth.Infrastructure.Repositories;

public class ProfessionalRepository : IProfessionalRepository
{
    private readonly AuthDbContext _dbContext;
    public ProfessionalRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Professional>> GetAllByEntityIdAsync(string entityId)
    {
        bool parsed = Guid.TryParse(entityId, out var id);
        if (!parsed)
            return new List<Professional>();
        
        List<Professional> professionals = await _dbContext.Set<Professional>()
            .AsNoTracking()
            .Where(p => EF.Property<Guid>(p, "_entityId") == id)
            .ToListAsync();
        
        return professionals;
    }

    public async Task<List<Professional>> GetAllAsync()
    {
        return await _dbContext.Set<Professional>()
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync();
    }
}