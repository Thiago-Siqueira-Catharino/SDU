namespace SDU.Contexts.Auth.Application.UseCases.UploadUseCase;

public class UploadFileOutput
{
    public Guid FileId { get; set; }
    public DateTimeOffset UploadedAt { get; set; }
}