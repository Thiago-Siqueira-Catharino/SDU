namespace SDU.Contexts.Files.Domain.Entities;

public class File
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    private Guid _uploaderId;
    private Guid _citizenId;
    public string Path { get; private set; }

    private void SetUploaderId(Guid userId)
    {
        _uploaderId = userId;
    }
    
    public Guid GetUploaderId()
    {
        return _uploaderId;
    }

    private void SetCitizenId(Guid citizenId)
    {
        _citizenId = citizenId;
    }

    public Guid GetCitizenId()
    {
        return _citizenId;
    }

    public File()
    {
        
    }
    
    public File(
        Guid uploaderId,
        Guid citizenId,
        string path
        )
    {
        if (uploaderId == Guid.Empty)
            throw new ArgumentException("O ID do reponsável pelo  upload é obrigatório.", nameof(uploaderId));

        if (citizenId == Guid.Empty)
            throw new ArgumentException("O ID do cidadão não pode ser vazio", nameof(citizenId));
        
        if (String.IsNullOrEmpty(path))
            throw new ArgumentException("Caminho de arquivo necessário", nameof(path));
        
        SetUploaderId(uploaderId);
        SetCitizenId(citizenId);
        Path = path;
    }
}