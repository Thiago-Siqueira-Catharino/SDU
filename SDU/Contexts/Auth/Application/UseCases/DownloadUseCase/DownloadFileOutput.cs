namespace SDU.Contexts.Auth.Application.UseCases.DownloadUseCase;

public class DownloadFileOutput : IDisposable // permite limpeza manual da memória
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public Stream ContentStream { get; set; }

    public void Dispose() // limpeza manual da memória
    {
        ContentStream?.Dispose();
    }
}