namespace SDU.Contexts.Auth.Application.UseCases.GetCitizens;

public record GetCitizensRequestDto(string? Name, string? Email, string? Cpf);