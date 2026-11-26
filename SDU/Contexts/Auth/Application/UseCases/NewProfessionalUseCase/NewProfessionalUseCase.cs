using Microsoft.AspNetCore.Identity;
using SDU.Contexts.Auth.Domain.Entities;

namespace SDU.Contexts.Auth.Application.UseCases.NewProfessionalUseCase;

public class NewProfessionalUseCase(UserManager<User> _userManager)
{
    public async Task Run(NewProfessionalDto newProfessionalDto, Guid entityId)
    {
        if (String.IsNullOrEmpty(newProfessionalDto.name))
            throw new ArgumentNullException(nameof(newProfessionalDto.name));
        
        if (String.IsNullOrEmpty(newProfessionalDto.email))
            throw new ArgumentNullException(nameof(newProfessionalDto.email));
        
        if (String.IsNullOrEmpty(newProfessionalDto.password))
            throw new ArgumentNullException(nameof(newProfessionalDto.password));
        
        if (String.IsNullOrEmpty(newProfessionalDto.cpf) && String.IsNullOrEmpty(newProfessionalDto.cim))
            throw new ArgumentException("Must have at least cim OR cpf");

        if (entityId == Guid.Empty)
            throw new ArgumentException("Id da entidade não encontrado");

        User entityUser = await _userManager.FindByIdAsync(entityId.ToString());
        if (entityUser == null)
            throw new ArgumentException("Entidade não encontrada");
        
        Entity? entity = entityUser as Entity;
        if (entity == null)
            throw new ArgumentException("Entidade não encontrada");

        Professional newProfessional = new Professional
        {
            UserName = newProfessionalDto.email,
            Name = newProfessionalDto.name,
            Email = newProfessionalDto.email,
        };
        
        if (!String.IsNullOrEmpty(newProfessionalDto.cpf))
            newProfessional.SetCpf(newProfessionalDto.cpf);
        
        if (!String.IsNullOrEmpty(newProfessionalDto.cim))
            newProfessional.SetCim(newProfessionalDto.cim);
        
        newProfessional.SetEntityId(entityId);
        
        var result = await _userManager.CreateAsync(newProfessional, newProfessionalDto.password);
        
        if (!result.Succeeded)
            throw new ArgumentException(result.Errors.First().Description);
        
        await _userManager.AddToRoleAsync(newProfessional, "Professional");
    }
}