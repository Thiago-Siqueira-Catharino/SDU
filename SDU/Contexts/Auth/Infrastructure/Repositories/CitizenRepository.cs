using Microsoft.EntityFrameworkCore;
using SDU.Contexts.Auth.Domain.Entities;
using SDU.Contexts.Auth.Domain.IRepositories;
using SDU.Contexts.Auth.Infrastructure.Persistance;

namespace SDU.Contexts.Auth.Infrastructure.Repositories;

public class CitizenRepository : ICitizenRepository
{
    private readonly AuthDbContext _dbContext;
    public CitizenRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Citizen>> ListCitizens(string? name, string? email, string? cpf)
    {
        IQueryable<Citizen> query = _dbContext.Citizens.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(c => c.Name == name);

        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(c => (c.UserName ?? c.Email) == email);

        if (!string.IsNullOrWhiteSpace(cpf))
            query = query.Where(c => EF.Property<string>(c, "_cpf") == cpf);
        
        return await query.OrderBy(c => c.Name).ToListAsync();
    }
}