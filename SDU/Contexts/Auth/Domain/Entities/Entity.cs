namespace SDU.Contexts.Auth.Domain.Entities;

public class Entity : User
{
    private string _cnpj;
    public int FileAmount { get; set; }
    public long TotalUsedStorage { get; set; }

    public void SetCnpj(string cnpj)
    {
        _cnpj = cnpj;
    }
    
    public string GetCnpj()
    {
        return _cnpj;
    }
}