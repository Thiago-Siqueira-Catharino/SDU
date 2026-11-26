namespace SDU.Contexts.Auth.Domain.Entities;

public class Professional : User
{
    private string _cpf;
    private string _cim;
    private Guid _entityId;
    
    public void SetCpf(string cpf)
    {
        _cpf = cpf;
    }

    public string GetCpf()
    {
        return _cpf;
    }

    public void SetCim(string cim)
    {
        _cim = cim;
    }

    public string GetCim()
    {
        return _cim;
    }

    public void SetEntityId(Guid entityId)
    {
        if (entityId == Guid.Empty)
            throw new ArgumentException("Id da entidade não pode ser vazio", nameof(entityId));
            
        _entityId = entityId;
    }

    public Guid GetEntityId()
    {
        return _entityId;
    }
}