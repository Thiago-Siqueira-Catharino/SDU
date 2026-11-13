namespace SDU.Contexts.Appointments.Domain.IRepositories;

public record AppointmentFilter(
    Guid? CitizenId = null,
    Guid? ProfessionalId = null,
    DateTime? Date = null
    );