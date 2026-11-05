namespace SDU.Contexts.Files.Application.UseCases.UploadUseCase;

public record UploadDto(
    string ProfessionalId,
    string CivilianId,
    IFormFile File
);
    
