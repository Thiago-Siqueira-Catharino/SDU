namespace SDU.Contexts.Auth.Application.UseCases.NewCitizenUseCase;

public record NewCitizenDto(
    string name,
    string email,
    string password,
    string cpf
    );