using Microsoft.AspNetCore.StaticFiles;
using SDU.Contexts.Auth.Domain.IRepositories;
using File = SDU.Contexts.Files.Domain.Entities.File;
using IFileRepository = SDU.Contexts.Files.Domain.IRepositories.IFileRepository;

namespace SDU.Contexts.Files.Application.UseCases.DownloadUseCase;

public class DownloadUseCase
{
    private readonly IFileRepository _fileRepository;
    private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider = new ();
    //private readonly IStorageService _storageService;
    public DownloadUseCase(IFileRepository fileRepository) //IStorageService storageService)
    {
        _fileRepository = fileRepository;
        //_storageService = storageService;
    }

    public async Task<DownloadResultDto> Run(DownloadDto dto, Guid userId, string userType)
    {
        if (userType == string.Empty)
            throw new ArgumentException("O tipo de usuário não pode ser vazio");
        
        if (!Guid.TryParse(dto.FileId, out Guid filedId))
            throw new ArgumentException("Guid inválido");
        
        var file = await _fileRepository.GetByIdAsync(filedId);
        if (file == null)
            throw new FileNotFoundException("O arquivo requisitado não exite no banco");

        if (userType == "Citizen" && userId != file.GetCitizenId())
            throw new ArgumentException("Você não tem autorização para baixar este arquivo");
        
        var path = file.Path;
        if (String.IsNullOrWhiteSpace(path))
            throw new ArgumentException("O caminho do araquivo requisitado não existe");
        
        if (!System.IO.File.Exists(path))
            throw new FileNotFoundException("O arquivo requisitado não existe");
        
        var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        var name = Path.GetFileName(path);
        if (!_fileExtensionContentTypeProvider.TryGetContentType(path, out var contentType))
        {
            contentType = "application/octet-stream"; 
        }
        return new DownloadResultDto(stream, contentType, name);
    }
}