using SDU.Contexts.Auth.Domain.Entities;

namespace SDU.Contexts.Appointments.Domain.Entities;

public class Appointment
{
    public Guid Id { get; private set; } = new Guid();
    public DateTime Date { get; private set; }
    public Guid CitizenId { get; private set; }
    public Guid ProfessionId { get; private set; }
    public Guid EntityId { get; private set; }

    public Appointment(
        Guid patientId,
        Guid professionId,
        Guid entityId,
        DateTime date
        )
    {
        Date = date;
        CitizenId = patientId;
        ProfessionId = professionId;
        EntityId = entityId;
    }
}