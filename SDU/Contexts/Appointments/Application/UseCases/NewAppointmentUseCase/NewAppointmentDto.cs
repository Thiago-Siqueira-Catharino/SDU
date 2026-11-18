namespace SDU.Contexts.Appointments.Application.UseCases.NewAppointmentUseCase;

public record NewAppointmentDto(
    DateTime Date,
    Guid PatientId,
    Guid ProfessionId,
    Guid EntityId
    );