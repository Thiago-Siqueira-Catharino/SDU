using SDU.Contexts.Auth.Domain.IRepositories;

namespace SDU.Contexts.Auth.Application.UseCases.DownloadUseCase;

public class DownloadFileUseCase
{
    private readonly IFileRepository _fileRepository;
    private readonly IStorageService _storageService;

    public DownloadFileUseCase(
        IFileRepository fileRepository, 
        IStorageService storageService)
    {
        _fileRepository = fileRepository;
        _storageService = storageService;
    }

    public async Task<DownloadFileOutput> ExecuteAsync(DownloadFileInput input)
    {
        var fileEntity = await _fileRepository.GetByIdAsync(input.FileId);

        if (fileEntity == null || fileEntity.IsDeleted)
        {
            throw new Exception("Arquivo não encontrado."); 
        }
        
        if (fileEntity.OwnerId != input.RequesterId)
        {
            throw new Exception("Você não tem permissão para baixar este arquivo."); 
        }
        
        var contentStream = await _storageService.GetFileStreamAsync(fileEntity.StoragePath);
        
        return new DownloadFileOutput
        {
            FileName = fileEntity.FileName,
            ContentType = fileEntity.ContentType,
            ContentStream = contentStream
        };
    }
}