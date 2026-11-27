using SDU.Contexts.Auth.Contracts;
using SDU.Contexts.Files.Application.UseCases.Upload;
using SDU.Contexts.Files.Domain.Entities;
using SDU.Contexts.Files.Domain.IRepositories;
using File = SDU.Contexts.Files.Domain.Entities.File;

namespace SDU.Contexts.Files.Application.UseCases.DiagnosticUpload;

public class DiagnosticUploadUseCase : UploadUseCase<DiagnosticUploadDto>    
{
    public DiagnosticUploadUseCase(IFileRepository fileRepository, IAuthModuleApi authModuleApi) 
        : base(fileRepository, authModuleApi)
    {
    }

    protected override File BuildFile(
        DiagnosticUploadDto dto, 
        Guid uploaderId, 
        Guid citizenId, 
        Guid entityId, 
        string path)
    {
        return new Diagnostic(uploaderId, citizenId, entityId, dto.Cid, path);
    }
}