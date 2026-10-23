using SDU.Contexts.Auth.Domain.Entities;

namespace SDU.Contexts.Auth.Domain.IRepositories;

public interface IFileRepository
{
    Task AddAsync(CitizenFile file);
    Task<CitizenFile> GetByIdAsync(Guid id);
}