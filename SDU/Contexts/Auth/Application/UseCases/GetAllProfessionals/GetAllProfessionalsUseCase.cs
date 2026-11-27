using SDU.Contexts.Auth.Domain.Entities;
using SDU.Contexts.Auth.Domain.IRepositories;

namespace SDU.Contexts.Auth.Application.UseCases.GetAllProfessionals;

public class GetAllProfessionalsUseCase
{
    private readonly IProfessionalRepository _professionalRepository;
    public GetAllProfessionalsUseCase(IProfessionalRepository professionalRepository)
    {
        _professionalRepository = professionalRepository;
    }

    public async Task<List<GetAllProfessionalsResponseDto>> Run()
    {
        List<GetAllProfessionalsResponseDto> response = new List<GetAllProfessionalsResponseDto>();
        foreach (Professional p in await _professionalRepository.GetAllAsync())
        {
            response.Add(
                new GetAllProfessionalsResponseDto(
                    p.Id,
                    p.Name,
                    p.GetCim(),
                    p.UserName ?? p.Email ?? p.NormalizedEmail.ToLower()
                )
            );
        }
        
        return response;
    }
}