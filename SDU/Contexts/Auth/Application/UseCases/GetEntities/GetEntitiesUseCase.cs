using SDU.Contexts.Auth.Domain.Entities;
using SDU.Contexts.Auth.Domain.IRepositories;

namespace SDU.Contexts.Auth.Application.UseCases.GetEntities;

public class GetEntitiesUseCase
{
    IEntityRepository _entityRepository;
    public GetEntitiesUseCase(IEntityRepository entityRepository)
    {
        _entityRepository = entityRepository;
    }

    public async Task<List<GetEntitiesResponseDto>> Run(GetEntitiesRequestDto request)
    {
        List<Entity> entityList = await _entityRepository.ListEntitesAsync(request.Name, request.Email);
        List<GetEntitiesResponseDto> response = new List<GetEntitiesResponseDto>();

        foreach (Entity e in entityList)
        {
            var dto = new GetEntitiesResponseDto(e.Name, e.Email ?? e.UserName);
            response.Add(dto);
        }
        
        return response;
    }
}