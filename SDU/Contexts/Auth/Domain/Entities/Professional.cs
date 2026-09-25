namespace SDU.Contexts.Auth.Domain.Entities;

public class Professional
{
    private string _cpf;
    private string _cim;
    
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
}