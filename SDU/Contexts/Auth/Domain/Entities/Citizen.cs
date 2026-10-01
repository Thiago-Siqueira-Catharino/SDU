namespace SDU.Contexts.Auth.Domain.Entities;

public class Citizen : User
{
    private string? _cpf;
    
    public void SetCpf(string cpf)
    {
        _cpf = cpf;
    }

    public string GetCpf()
    {
        return _cpf;
    }
}