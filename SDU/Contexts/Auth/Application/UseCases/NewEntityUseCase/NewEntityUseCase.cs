using Microsoft.AspNetCore.Identity;
using SDU.Contexts.Auth.Domain.Entities;

namespace SDU.Contexts.Auth.Application.UseCases.NewEntityUseCase;

public class NewEntityUseCase(UserManager<User> userManager)
{
    public async Task Run(NewEntityDto newEntityDto)
    {
        if (string.IsNullOrEmpty(newEntityDto.Name))
            throw new ArgumentNullException(nameof(newEntityDto.Name));
        
        if (string.IsNullOrEmpty(newEntityDto.Email))
            throw new ArgumentNullException(nameof(newEntityDto.Email));
        
        if (string.IsNullOrEmpty(newEntityDto.Password))
            throw new ArgumentNullException(nameof(newEntityDto.Password));
        
        if (string.IsNullOrEmpty(newEntityDto.Cnpj))
            throw new ArgumentNullException(nameof(newEntityDto.Cnpj));
        
        Entity newEntity = new Entity
        {
            UserName =  newEntityDto.Email,
            Email = newEntityDto.Email,
            Name = newEntityDto.Name,
        };
        newEntity.SetCnpj(newEntityDto.Cnpj);
        
        var result = await userManager.CreateAsync(newEntity, newEntityDto.Password);
        
        if (!result.Succeeded)
            throw new ArgumentException(result.Errors.First().Description);
        
        await userManager.AddToRoleAsync(newEntity, "Entity");
    }
}