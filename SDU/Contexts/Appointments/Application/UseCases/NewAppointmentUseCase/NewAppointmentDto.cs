namespace SDU.Contexts.Appointments.Application.UseCases.NewAppointmentUseCase;

public record NewAppointmentDto(
    DateTime Date,
    Guid PatientId,
    Guid ProfessionalId,
    Guid EntityId
    );