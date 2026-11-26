namespace SDU.Contexts.Auth.Contracts;

public interface IAuthModuleApi
{
    Task<Guid?> GetEntityIdByProfessionalIdAsync(string professionalId);
    Task RegisterFileUploadAsync(Guid entityId, long fileSizeinBytes);
    
}