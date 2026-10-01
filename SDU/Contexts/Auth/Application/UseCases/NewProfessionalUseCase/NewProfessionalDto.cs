namespace SDU.Contexts.Auth.Application.UseCases.NewProfessionalUseCase;

public record NewProfessionalDto(
    string name,
    string email,
    string password,
    string cpf,
    string cim
    );