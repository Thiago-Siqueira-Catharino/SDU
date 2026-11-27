namespace SDU.Contexts.Files.Domain.Entities;

public class Diagnostic : File
{
    public string Cid { get; set; }

    public Diagnostic()
    {
    }
    
    public Diagnostic(Guid uploaderId,
        Guid citizenId,
        Guid entityId,
        string cid,
        string path) : base(uploaderId: uploaderId, citizenId: citizenId, entityId: entityId, path: path)
    {
        if (string.IsNullOrWhiteSpace(cid))
            throw new ArgumentNullException("Especifique o CID do diagnostico");
        
        Cid = cid;
    }
}