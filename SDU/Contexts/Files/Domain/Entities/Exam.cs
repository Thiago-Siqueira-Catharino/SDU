namespace SDU.Contexts.Files.Domain.Entities;

public class Exam : File 
{
    public string ExamType { get; set; }

    public Exam()
    {
    }
    
    public Exam(Guid uploaderId,
        Guid citizenId,
        Guid entityId,
        string path,
        string examType) : base(uploaderId, citizenId, entityId, path)
    {
        if (string.IsNullOrEmpty(examType))
            throw new ArgumentNullException("Tipo de exame não pode ser nulo");
        
        ExamType = examType.ToUpper();
    }
}