using Microsoft.AspNetCore.Identity;
using SDU.Contexts.Auth.Domain.Entities;

namespace SDU.Contexts.Auth.Application.UseCases.NewCitizenUseCase;

public class NewCitizenUseCase(UserManager<User> _UserManager)
{
    public async Task Run(NewCitizenDto newCitizenDto)
    {
        if (String.IsNullOrEmpty(newCitizenDto.name))
            throw new ArgumentNullException(nameof(newCitizenDto.name));
        
        if (String.IsNullOrEmpty(newCitizenDto.email))
            throw new ArgumentNullException(nameof(newCitizenDto.email));
        
        if (String.IsNullOrEmpty(newCitizenDto.password))
            throw new ArgumentNullException(nameof(newCitizenDto.password));
        
        if (String.IsNullOrEmpty(newCitizenDto.cpf))
            throw new ArgumentNullException(nameof(newCitizenDto.cpf));
        
        Citizen newCitizen = new Citizen
        {
            UserName = newCitizenDto.email,
            Name = newCitizenDto.name,
            Email = newCitizenDto.email,
        };
        
        newCitizen.SetCpf(newCitizenDto.cpf);

        var result = await _UserManager.CreateAsync(newCitizen, newCitizenDto.password);
        
        if (!result.Succeeded)
            throw new ArgumentException(result.Errors.First().Description);
        
        await _UserManager.AddToRoleAsync(newCitizen, "Citizen");
    }
}