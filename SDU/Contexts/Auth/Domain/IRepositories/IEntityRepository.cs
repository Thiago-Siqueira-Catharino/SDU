using SDU.Contexts.Auth.Domain.Entities;

namespace SDU.Contexts.Auth.Domain.IRepositories;

public interface IEntityRepository
{
    Task IncrementBytesAsync(string entityId, long bytes);
    Task IncrementFileCounterAsync(string entityId);
    Task RegisterFileAsync(string entityId, long bytes);
    Task<List<Entity>> ListEntitesAsync(string? name, string? email);
}