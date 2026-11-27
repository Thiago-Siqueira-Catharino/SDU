namespace SDU.Contexts.Files.Domain.Entities;

public abstract class File
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    private Guid _uploaderId;
    private Guid _entityId;
    private Guid _citizenId;
    public DateTime UploadDate { get; private set; } =  DateTime.UtcNow;
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

    private void SetEntityId(Guid entityId)
    {
        _entityId = entityId;
    }

    private Guid GetEntityId()
    {
        return _entityId;
    }

    public File()
    {
        
    }
    
    protected File(
        Guid uploaderId,
        Guid citizenId,
        Guid entityId,
        string path
        )
    {
        if (uploaderId == Guid.Empty)
            throw new ArgumentException("O ID do reponsável pelo  upload é obrigatório.", nameof(uploaderId));

        if (citizenId == Guid.Empty)
            throw new ArgumentException("O ID do cidadão não pode ser vazio", nameof(citizenId));
        
        if (entityId == Guid.Empty)
            throw new ArgumentException("O ID da isntituição não pode ser vazio", nameof(entityId));
        
        if (String.IsNullOrEmpty(path))
            throw new ArgumentException("Caminho de arquivo necessário", nameof(path));
        
        SetUploaderId(uploaderId);
        SetCitizenId(citizenId);
        SetEntityId(entityId);
        Path = path;
    }
}