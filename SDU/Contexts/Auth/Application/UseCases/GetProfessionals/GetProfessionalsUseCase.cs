using SDU.Contexts.Auth.Domain.Entities;
using SDU.Contexts.Auth.Domain.IRepositories;

namespace SDU.Contexts.Auth.Application.UseCases.GetProfessionals;

public class GetProfessionalsUseCase(IProfessionalRepository repository)
{
    public async Task<List<ProfessionalDto>> Run(string entityId)
    {
        List<Professional> professionals = await repository.GetAllByEntityIdAsync(entityId);

        List<ProfessionalDto> reponse = new List<ProfessionalDto>();
        foreach (var professional in professionals)
        {
            reponse.Add(
                new ProfessionalDto(
                    professional.Id,  
                    professional.Name, 
                    professional.Email, 
                    professional.GetCim()
                )
            );
        }
        
        return reponse;
    }
}