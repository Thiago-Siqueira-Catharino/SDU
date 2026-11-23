using SDU.Contexts.Appointments.Domain.Entities;
using SDU.Contexts.Appointments.Domain.IRepositories;

namespace SDU.Contexts.Appointments.Application.UseCases.SearchOwnAppointments;

public record SearchOwnAppointmentsDto(
    ComparisonMode? Mode,
    AppointmentStatus? Status
    );