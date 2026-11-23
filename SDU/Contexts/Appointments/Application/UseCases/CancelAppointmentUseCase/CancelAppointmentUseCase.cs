using SDU.Contexts.Appointments.Domain.IRepositories;

namespace SDU.Contexts.Appointments.Application.UseCases.CancelAppointmentUseCase;

public class CancelAppointmentUseCase
{
    private readonly IAppointmentRepository _appointmentRepository;
    public CancelAppointmentUseCase(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }
    
    public async Task Run(Guid appointmentId, Guid callerId)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null)
            throw new InvalidOperationException("A consulta solicitada não existe");

        if (callerId != appointment.CitizenId &&
            callerId != appointment.ProfessionalId &&
            callerId != appointment.EntityId)
            throw new InvalidOperationException("Você não tem permissão para cancelar essa consulta");
        
        appointment.Cancelar();
        await _appointmentRepository.SaveChangesAsync();
    }
}