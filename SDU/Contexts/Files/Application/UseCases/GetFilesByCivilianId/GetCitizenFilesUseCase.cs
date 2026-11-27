using SDU.Contexts.Files.Domain.IRepositories;

namespace SDU.Contexts.Files.Application.UseCases.GetExamFilesByCivilianId;


public class GetCitizenFilesUseCase
{
    private readonly IFileRepository _fileRepository;
    public GetCitizenFilesUseCase(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<List<GetCitizenFilesResponseDto>> Run(string citizenId)
    {
        if (string.IsNullOrEmpty(citizenId))
            throw new ArgumentNullException("É necessário o Id para pesquisar os arquivos");

        if (!Guid.TryParse(citizenId, out var citizenGuid))
            throw new ArgumentException("Id invalido");
        
        var files = await _fileRepository.GetByCitizenIdAsync(citizenGuid);

        List<GetCitizenFilesResponseDto> response = new List<GetCitizenFilesResponseDto>();
        foreach (var file in files)
        {
            response.Add(
                new GetCitizenFilesResponseDto(file.Id, file.GetType().Name, file.UploadDate)
            );
        }
        return response;
    }
}