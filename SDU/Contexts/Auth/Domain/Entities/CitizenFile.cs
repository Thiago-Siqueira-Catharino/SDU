namespace SDU.Contexts.Auth.Domain.Entities;

public class CitizenFile
{
        public Guid Id { get; private set; }
        public Guid OwnerId { get; private set; }
        public Guid UploaderId { get; private set; }
        
        public string FileName { get; private set; }
        public string ContentType { get; private set; }
        public long Size { get; private set; } //em bytes
        public string StoragePath { get; private set; }
        public DateTimeOffset UploadedAt { get; private set; }
        public bool IsDeleted { get; private set; }
        
        private CitizenFile() { }
        
        public CitizenFile(
            Guid ownerId, 
            Guid uploaderId, 
            string fileName, 
            string contentType, 
            long size, 
            string storagePath)
        {
            if (ownerId == Guid.Empty) 
                throw new ArgumentException("O ID do dono é obrigatório.", nameof(ownerId));
                
            if (uploaderId == Guid.Empty) 
                throw new ArgumentException("O ID do responsável pelo upload é obrigatório.", nameof(uploaderId));
                
            if (string.IsNullOrWhiteSpace(fileName)) 
                throw new ArgumentException("O nome do arquivo é obrigatório.", nameof(fileName));

            if (size <= 0)
                throw new ArgumentException("O tamanho do arquivo deve ser maior que zero.", nameof(size));

            if (string.IsNullOrWhiteSpace(storagePath))
                throw new ArgumentException("O caminho de armazenamento é obrigatório.", nameof(storagePath));

            Id = Guid.NewGuid();
            OwnerId = ownerId;
            UploaderId = uploaderId;
            FileName = fileName;
            ContentType = contentType;
            Size = size;
            StoragePath = storagePath;
            UploadedAt = DateTimeOffset.UtcNow;
            IsDeleted = false;
        }

        public void TransferOwnership(Guid newOwnerId)
        {
            if (newOwnerId == Guid.Empty)
                throw new ArgumentException("O novo ID de dono não pode ser vazio!");
                
            OwnerId = newOwnerId;
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }
}