using Microsoft.AspNetCore.Identity;
using SDU.Contexts.Auth.Domain.Entities;

namespace SDU.Contexts.Auth.Application.UseCases.GetDashboardStats;

public class GetDashboardStatsUseCase(UserManager<User> userManager)
{
    public async Task<GetDashboardStatsResponseDto> Run(Guid entityId)
    {
        string strId = entityId.ToString();
        User? entityUser = await userManager.FindByIdAsync(strId);
        if (entityUser is null)
            throw new ArgumentException("Entidade não encontrada");
        
        Entity? entity = entityUser as Entity;
        if (entity is null)
            throw new ArgumentException("Entidade não encontrada");

        int fileAmount = entity.FileAmount;
        double totalUsedStorage = entity.TotalUsedStorage / 1024.0 / 1024.0 / 1024.0;
        string cnpj = entity.GetCnpj();
        string name = entity.Name;
        return new GetDashboardStatsResponseDto(
            FileAmount:fileAmount, 
            TotalUsedStorage: totalUsedStorage, 
            CNPJ: cnpj, 
            Name: name
        );
    }
}