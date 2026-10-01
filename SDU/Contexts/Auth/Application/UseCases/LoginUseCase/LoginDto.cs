namespace SDU.Contexts.Auth.Application.UseCases.LoginUseCase;

public record LoginDto(
    string email,
    string password
    );