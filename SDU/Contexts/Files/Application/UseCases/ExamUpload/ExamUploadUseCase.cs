using SDU.Contexts.Auth.Contracts;
using SDU.Contexts.Files.Application.UseCases.Upload;
using SDU.Contexts.Files.Domain.Entities;
using SDU.Contexts.Files.Domain.IRepositories;
using File = SDU.Contexts.Files.Domain.Entities.File;

namespace SDU.Contexts.Files.Application.UseCases.ExamUpload;

public class ExamUploadUseCase : UploadUseCase<ExamUploadDto>
{
    public ExamUploadUseCase(IFileRepository fileRepository, IAuthModuleApi authModuleApi) 
        : base(fileRepository, authModuleApi)
    {
    }

    protected override File BuildFile(
        ExamUploadDto dto, 
        Guid uploaderId, 
        Guid citizenId, 
        Guid entityId, 
        string path)
    {
        return new Exam(
            uploaderId: uploaderId, 
            citizenId: citizenId, 
            entityId: entityId, 
            path: path, 
            examType: dto.ExamType
        );
    }
}