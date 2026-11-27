using SDU.Contexts.Auth.Domain.Entities;
using SDU.Contexts.Auth.Domain.IRepositories;

namespace SDU.Contexts.Auth.Application.UseCases.GetCitizens;

public class GetCitizensUseCase
{
    ICitizenRepository _citizenRepository;
    public GetCitizensUseCase(ICitizenRepository citizenRepository)
    {
        _citizenRepository = citizenRepository;
    }

    public async Task<List<GetCitizensResponseDto>> Run(GetCitizensRequestDto request)
    {
        List<Citizen> citizenList = await _citizenRepository.ListCitizens(request.Name, request.Email, request.Cpf);
        List<GetCitizensResponseDto> response = new List<GetCitizensResponseDto>();
        foreach (var c in citizenList)
        {
            Guid.TryParse(c.Id, out Guid citizenId);
            response.Add(
                new GetCitizensResponseDto(
                    citizenId,
                    c.Name,
                    c.UserName ?? c.Email
                )
            );
        }

        return response;
    }
}