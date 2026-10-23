namespace SDU.Contexts.Auth.Domain.IRepositories;

public interface IStorageService
{
    Task<string> SaveFileAsync(string fileName, Stream contentStream);
    Task<Stream> GetFileStreamAsync(string storagePath);
}