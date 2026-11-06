namespace SDU.Contexts.Files.Application.UseCases.DownloadUseCase;

public record DownloadResultDto(
    Stream FileStream,
    string ContentType,
    string FileName
    );