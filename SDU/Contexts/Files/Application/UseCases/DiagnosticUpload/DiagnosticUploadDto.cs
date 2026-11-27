using SDU.Contexts.Files.Application.UseCases.Upload;

namespace SDU.Contexts.Files.Application.UseCases.DiagnosticUpload;

public record DiagnosticUploadDto : UploadDto
{
    public string Cid { get; set; }
};