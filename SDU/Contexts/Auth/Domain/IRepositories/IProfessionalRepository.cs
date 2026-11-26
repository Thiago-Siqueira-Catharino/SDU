using SDU.Contexts.Auth.Domain.Entities;

namespace SDU.Contexts.Auth.Domain.IRepositories;

public interface IProfessionalRepository
{
    Task<List<Professional>> GetAllByEntityIdAsync(string entityId);
}