namespace SDU.Contexts.Auth.Domain.Entities;

public class Entity
{
    private string _cnpj;

    public void SetCnpj(string cnpj)
    {
        _cnpj = cnpj;
    }
    
    public string GetCnpj()
    {
        return _cnpj;
    }
}