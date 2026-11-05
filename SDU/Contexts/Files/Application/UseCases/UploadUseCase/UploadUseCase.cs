using SDU.Contexts.Files.Domain.IRepositories;
using File = SDU.Contexts.Files.Domain.Entities.File;

namespace SDU.Contexts.Files.Application.UseCases.UploadUseCase;

public class UploadUseCase
{
    private readonly IFileRepository _fileRepository;
    public UploadUseCase(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }
    
    private static readonly HashSet<String> ValidTypes = [
        "image/png",
        "application/pdf",
        "application/msword",                                                             
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document"    
    ];

    private static readonly HashSet<String> ValidExtensions = [
        ".png",
        ".pdf",
        ".doc",
        ".docx"
    ];
    private Boolean TooBig(IFormFile file)
    {
        const long maxSize = 50 * 1024 * 1024;
        if (file.Length > maxSize)
            return true;
        
        return false;
    }

    private Boolean InvalidType(IFormFile file)
    {
        string type =  file.ContentType.ToLower();
        if (!ValidTypes.Contains(type))
            return true;
        
        string extension = Path.GetExtension(file.FileName).ToLower();
        if (!ValidExtensions.Contains(extension))
            return true;
        
        return false;
    }

    private String Rename(IFormFile file)
    {
        string extension = Path.GetExtension(file.FileName);
        return $"{Guid.NewGuid()}{extension}";
    }
    
    public async Task Run(UploadDto uploadDto)
    {
        //puxa o arquivo do DTO
       IFormFile file = uploadDto.File;
       
       //verificação de tamanho
       if (TooBig(file))
           throw new ArgumentException("File is too large");
       
       //verificação de tipo
       if (InvalidType(file))
           throw new ArgumentException("File is invalid. Accepted types: .png, .pdf, .docx, .doc");
       
       //Começa a construir o caminho
       var folder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
       
       //Contruindo o nome
       string newName = Rename(file);
       
       //Termina de construir o caminho
       var fullPath = Path.Combine(folder, newName);
       
       //Pegando os GUIDs
       if (!Guid.TryParse(uploadDto.ProfessionalId, out Guid uploadId) || uploadId == Guid.Empty)
           throw new ArgumentException("Professional id inválido");
       
       if (!Guid.TryParse(uploadDto.CivilianId, out Guid citizenId) ||  citizenId == Guid.Empty)
           throw new ArgumentException("Citizen id inválido");
       
       //Tenta montar o novo objeto para salvar no DB
       File toSave = new File(
           uploaderId: uploadId, 
           citizenId: citizenId, 
           path: fullPath
           );
       if (toSave == null)
           throw new ArgumentException("Erro ao montar objeto");
       
       //Salva no disco
       await using (var stream = new FileStream(fullPath, FileMode.Create)) 
       {
            await file.CopyToAsync(stream);
       }

       try
       {
           //Tenta salvar no DB
           await _fileRepository.AddAsync(toSave);
       }
       catch (Exception e)
       {
           //Apaga se der B.O.
           System.IO.File.Delete(fullPath);
           Console.WriteLine(e);
           throw;
       }
       
    }
}