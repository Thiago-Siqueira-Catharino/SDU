namespace SDU.Contexts.Auth.Application.UseCases.DownloadUseCase;

public class DownloadFileInput
{
    public Guid FileId { get; set; }
    public Guid RequesterId { get; set; }
}