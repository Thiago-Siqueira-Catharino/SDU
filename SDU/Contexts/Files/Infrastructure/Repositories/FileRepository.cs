using Microsoft.EntityFrameworkCore;
using SDU.Contexts.Files.Domain.IRepositories;
using SDU.Contexts.Files.Infrastructure.Persistance;
using File = SDU.Contexts.Files.Domain.Entities.File;

namespace SDU.Contexts.Files.Infrastructure.Repositories;

public class FileRepository : IFileRepository
{
    private readonly FileDbContext _dbContext;

    public FileRepository(FileDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(File file)
    {
        await _dbContext.Files.AddAsync(file);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<File?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Files.FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<List<File>> GetByCitizenIdAsync(Guid citizenId)
    {
        return await _dbContext.Files.Where(f => 
            EF.Property<Guid>(f, "_citizenId") == citizenId)
            .ToListAsync();
    }

    public async Task<List<File>> GetByProfessionalIdAsync(Guid professionalId)
    {
        return await _dbContext.Files.Where(f =>
                EF.Property<Guid>(f, "uploaderId") == professionalId)
            .ToListAsync();
    }
}