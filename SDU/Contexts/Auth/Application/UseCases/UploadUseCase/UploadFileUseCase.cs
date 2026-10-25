using SDU.Contexts.Auth.Domain.Entities;
using SDU.Contexts.Auth.Domain.IRepositories;

namespace SDU.Contexts.Auth.Application.UseCases.UploadUseCase;

public class UploadFileUseCase
{
    private readonly IFileRepository _fileRepository;
    private readonly IStorageService _storageService;
    private readonly IProfessionalRepository _professionalRepository;

    public UploadFileUseCase(
        IFileRepository fileRepository, 
        IStorageService storageService,
        IProfessionalRepository professionalRepository)
    {
        _fileRepository = fileRepository;
        _storageService = storageService;
        _professionalRepository = professionalRepository;
    }

    public async Task<UploadFileOutput> ExecuteAsync(UploadFileInput input)
    {
        var isProfessional = await _professionalRepository.ExistsAsync(input.UploaderId);
        // mudar conforme o que for criado na interface do repositório
        
        if (!isProfessional)
        {
            throw new UnauthorizedAccessException("Apenas profissionais estão autorizados a fazer upload de arquivos.");
        }
        
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