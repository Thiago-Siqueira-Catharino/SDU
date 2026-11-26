using Microsoft.AspNetCore.Identity;
using SDU.Contexts.Auth.Application.UseCases.IncrementEntityCounterUseCase;
using SDU.Contexts.Auth.Contracts;
using SDU.Contexts.Auth.Domain.Entities;
using SDU.Contexts.Auth.Infrastructure.Persistance;

namespace SDU.Contexts.Auth.Infrastructure.Services;

public class AuthModuleApi : IAuthModuleApi
{
    private readonly UserManager<User> _userManager;
    private IncrementEntityCounterUseCase _incrementEntityCounterUseCase;
    private readonly AuthDbContext _authDbContext;
    public AuthModuleApi(
        UserManager<User> userManager, 
        IncrementEntityCounterUseCase incrementEntityCounterUseCase,
        AuthDbContext authDbContext
        )
    {
        _userManager = userManager;
        _incrementEntityCounterUseCase =  incrementEntityCounterUseCase;
        _authDbContext = authDbContext;
    }

    public async Task<Guid?> GetEntityIdByProfessionalIdAsync(string professionalId)
    {
        if (string.IsNullOrEmpty(professionalId))
            return null;
        
        if (!Guid.TryParse(professionalId, out _))
            return null;
        
        User? professionalUser = await _userManager.FindByIdAsync(professionalId);
        Professional professional = professionalUser as Professional;
        if (professional is null)
            return null;

        string entityId = professional.GetEntityId().ToString();
        if (string.IsNullOrEmpty(entityId))
            return null;

        User? entityUser = await _userManager.FindByIdAsync(entityId);
        Entity? entity = entityUser as Entity;
        if (entity is null)
            return  null;
        
        if(!Guid.TryParse(entityId, out Guid id))
            return null;
        
        return id;
    }

    public async Task RegisterFileUploadAsync(Guid entityId, long fileSizeinBytes)
    {
        _incrementEntityCounterUseCase.Run(entityId, fileSizeinBytes);
    }
    
    
}