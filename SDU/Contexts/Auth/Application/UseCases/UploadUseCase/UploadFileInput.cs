namespace SDU.Contexts.Auth.Application.UseCases.UploadUseCase;

public class UploadDocumentInput
{
    public Guid OwnerId { get; set; }
    public Guid UploaderId { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; } 
    public Stream ContentStream { get; set; }   // faz com que os bytes do arquivo sejam carregados em
                                                // pedaços, poupando memória RAM do server
}