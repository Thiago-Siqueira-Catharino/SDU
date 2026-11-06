using File = SDU.Contexts.Files.Domain.Entities.File;

namespace SDU.Contexts.Files.Domain.IRepositories;

public interface IFileRepository
{
    Task AddAsync(File file);
    Task<File?> GetByIdAsync(Guid id);
    Task<List<File>> GetByCitizenIdAsync(Guid citizenId);
    Task<List<File>> GetByProfessionalIdAsync(Guid professionalId);
}