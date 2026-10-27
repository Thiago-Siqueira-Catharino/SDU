using SDU.Contexts.Auth.Domain.Entities;
using SDU.Contexts.Auth.Domain.IRepositories;

namespace SDU.Contexts.Auth.Application.UseCases.UploadUseCase;

public class UploadFileUseCase
{
    private readonly IFileRepository _fileRepository;
    private readonly IStorageService _storageService;

    public UploadFileUseCase(
        IFileRepository fileRepository, 
        IStorageService storageService)
    {
        _fileRepository = fileRepository;
        _storageService = storageService;
    }

    public async Task<UploadFileOutput> ExecuteAsync(UploadFileInput input)
    {
        string generatedStoragePath = await _storageService.SaveFileAsync(input.FileName, input.ContentStream);
        
        var fileEntity = new CitizenFile( 
            ownerId: input.OwnerId,
            uploaderId: input.UploaderId,
            fileName: input.FileName,
            contentType: input.ContentType,
            size: input.Size,
            storagePath: generatedStoragePath
        );
        
        await _fileRepository.AddAsync(fileEntity);
        
        return new UploadFileOutput
        {
            FileId = fileEntity.Id,
            UploadedAt = fileEntity.UploadedAt
        };
    }
}