namespace SDU.Contexts.Auth.Application.UseCases.NewEntityUseCase;

public record NewEntityDto(
    string Name,
    string Email,
    string Password,
    string Cnpj
    );