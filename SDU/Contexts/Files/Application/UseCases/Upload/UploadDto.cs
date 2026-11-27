namespace SDU.Contexts.Files.Application.UseCases.Upload;

public record UploadDto
{
    public string ProfessionalId { get; set; }
    public string CivilianId { get; set; }
    public IFormFile File { get; set; }
};
    
