namespace SDU.Contexts.Auth.Application.UseCases.DownloadUseCase;

public class DownloadDocumentInput
{
    public Guid DocumentId { get; set; }
    public Guid RequesterId { get; set; }
}