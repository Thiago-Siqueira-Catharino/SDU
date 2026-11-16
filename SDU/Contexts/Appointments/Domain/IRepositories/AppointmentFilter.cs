namespace SDU.Contexts.Appointments.Domain.IRepositories;

public record AppointmentFilter(
    ComparisonMode Mode,
    Guid? CitizenId = null,
    Guid? ProfessionalId = null,
    DateTime? Date = null
);

public enum ComparisonMode
{
    Before,
    Around,
    After
}