namespace SDU.Contexts.Appointments.Domain.Entities;

public enum AppointmentStatus
{
    Agendado,
    Cancelado,
    Efetuado,
    Ausente
}

public class Appointment
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public DateTime Date { get; private set; }
    public Guid CitizenId { get; private set; }
    public Guid ProfessionalId { get; private set; }
    public Guid EntityId { get; private set; }
    public AppointmentStatus Status { get; private set; }

    private Appointment() { }

    public Appointment(Guid citizenId, Guid professionalId, Guid entityId, DateTime date)
    {
        if (date <= DateTime.UtcNow)
            throw new ArgumentException("A data do agendamento deve ser futura.", nameof(date));

        CitizenId = citizenId;
        ProfessionalId = professionalId;
        EntityId = entityId;
        Date = date;
        Status = AppointmentStatus.Agendado;
    }

    public void Cancelar()
    {
        EnsureAgendado("cancelar");
        Status = AppointmentStatus.Cancelado;
    }

    public void Efetuar()
    {
        EnsureAgendado("efetuar");
        EnsureDataPassada("efetuar");
        Status = AppointmentStatus.Efetuado;
    }

    public void MarcarAusente()
    {
        EnsureAgendado("marcar como ausente");
        EnsureDataPassada("marcar como ausente");
        Status = AppointmentStatus.Ausente;
    }

    private void EnsureAgendado(string acao)
    {
        if (Status != AppointmentStatus.Agendado)
            throw new InvalidOperationException(
                $"Não é possível {acao} um agendamento que está {Status}.");
    }

    private void EnsureDataPassada(string acao)
    {
        if (Date > DateTime.UtcNow)
            throw new InvalidOperationException(
                $"Não é possível {acao} um agendamento que ainda não aconteceu.");
    }
}