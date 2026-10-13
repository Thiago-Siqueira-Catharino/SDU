namespace SDU.Contexts.Auth.Application.UseCases.UploadUseCase;

public class UploadDocumentOutput
{
    public Guid DocumentId { get; set; }
    public DateTimeOffset UploadedAt { get; set; }
}