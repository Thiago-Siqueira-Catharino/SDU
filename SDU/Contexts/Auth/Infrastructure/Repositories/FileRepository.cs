using Microsoft.EntityFrameworkCore;
using SDU.Contexts.Auth.Domain.Entities;
using SDU.Contexts.Auth.Domain.IRepositories;
using SDU.Contexts.Auth.Infrastructure.Persistance;

namespace SDU.Contexts.Auth.Infrastructure.Repositories;

public class FileRepository : IFileRepository
{
    private readonly AuthDbContext _context;

    public FileRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CitizenFile file)
    {
        await _context.CitizenFiles.AddAsync(file);
        await _context.SaveChangesAsync();
    }

    public async Task<CitizenFile> GetByIdAsync(Guid id)
    {
        return await _context.CitizenFiles.FirstOrDefaultAsync(f => f.Id == id);
    }
}