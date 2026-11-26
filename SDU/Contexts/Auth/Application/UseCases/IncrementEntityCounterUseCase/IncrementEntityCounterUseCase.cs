using Microsoft.AspNetCore.Identity;
using SDU.Contexts.Auth.Domain.Entities;
using SDU.Contexts.Auth.Domain.IRepositories;

namespace SDU.Contexts.Auth.Application.UseCases.IncrementEntityCounterUseCase;

public class IncrementEntityCounterUseCase(
    UserManager<User> userManager,
    IEntityRepository entityRepository
    )
{
    public async Task Run(Guid entityId, long bytes)
    {
        string strId = entityId.ToString();
        var user =  await userManager.FindByIdAsync(strId);
        Entity? entity =  user as Entity;
        if (entity == null)
            throw new ArgumentNullException("Entidade solicitada não encontrada");
        
        await entityRepository.RegisterFileAsync(strId, bytes);
    }
}