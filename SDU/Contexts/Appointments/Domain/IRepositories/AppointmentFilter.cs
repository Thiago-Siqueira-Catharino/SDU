using SDU.Contexts.Appointments.Domain.Entities;

namespace SDU.Contexts.Appointments.Domain.IRepositories;

public record AppointmentFilter(
    ComparisonMode? Mode = null,
    AppointmentStatus? Status = null,
    Guid? CitizenId = null,
    Guid? ProfessionalId = null,
    Guid? EntityId = null,
    DateTime? Date = null
);

public enum ComparisonMode
{
    Before,
    Around,
    After
}