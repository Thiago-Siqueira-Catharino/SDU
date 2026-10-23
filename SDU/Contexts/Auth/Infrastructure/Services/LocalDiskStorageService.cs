using SDU.Contexts.Auth.Domain.IRepositories;

namespace SDU.Contexts.Auth.Infrastructure.Services;

public class LocalDiskStorageService : IStorageService
{
    private readonly string _baseStoragePath;

    public LocalDiskStorageService()
    {
        // Em um cenário real, busque este caminho do appsettings.json via IConfiguration
        _baseStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "CitizenFiles");
        
        if (!Directory.Exists(_baseStoragePath))
        {
            Directory.CreateDirectory(_baseStoragePath);
        }
    }

    public async Task<string> SaveFileAsync(string fileName, Stream contentStream)
    {
        // Gera um nome único para evitar sobrescrita de arquivos com o mesmo nome
        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var fullPath = Path.Combine(_baseStoragePath, uniqueFileName);

        using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
        {
            await contentStream.CopyToAsync(fileStream);
        }

        // Retorna o caminho relativo ou identificador para salvar no banco
        return uniqueFileName; 
    }

    public Task<Stream> GetFileStreamAsync(string storagePath)
    {
        var fullPath = Path.Combine(_baseStoragePath, storagePath);

        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException("O arquivo físico não foi encontrado no disco.", fullPath);
        }

        // Retorna o stream aberto (a camada de Application/Controller fará o Dispose)
        Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        return Task.FromResult(stream);
    }
}