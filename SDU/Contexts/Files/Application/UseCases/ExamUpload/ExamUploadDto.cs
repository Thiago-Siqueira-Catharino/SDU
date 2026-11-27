using SDU.Contexts.Files.Application.UseCases.Upload;

namespace SDU.Contexts.Files.Application.UseCases.ExamUpload;

public record ExamUploadDto : UploadDto
{
    public string ExamType { get; set; }
}