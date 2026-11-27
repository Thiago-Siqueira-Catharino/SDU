using SDU.Contexts.Auth.Domain.Entities;

namespace SDU.Contexts.Auth.Domain.IRepositories;

public interface ICitizenRepository
{
    Task<List<Citizen>> ListCitizens(string? name, string? email, string? cpf);
}