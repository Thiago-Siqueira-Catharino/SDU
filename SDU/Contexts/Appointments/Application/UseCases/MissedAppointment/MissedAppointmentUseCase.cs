using SDU.Contexts.Appointments.Domain.Entities;
using SDU.Contexts.Appointments.Domain.IRepositories;

namespace SDU.Contexts.Appointments.Application.UseCases.MissedAppointment;

public class MissedAppointmentUseCase
{
    private readonly IAppointmentRepository _appointmentRepository;
    public MissedAppointmentUseCase(IAppointmentRepository missedAppointmentUseCase)
    {
        _appointmentRepository = missedAppointmentUseCase;
    }
    
    private async Task<Appointment?> GetAppointment(Guid appointmentId)
    {
        return await _appointmentRepository.GetByIdAsync(appointmentId);
    }
    
    public async Task Run(Guid appointmentId, Guid calledId)
    {
        var appointment = await GetAppointment(appointmentId);
        if (appointment == null)
            throw new ArgumentException($"Consulta com id {appointmentId} não encontrada");

        if (calledId != appointment.ProfessionalId && calledId != appointment.EntityId)
            throw new InvalidOperationException("Você não tem permissão para marcar essa consulta como realizada");
        
        appointment.MarcarAusente();
        await _appointmentRepository.SaveChangesAsync();
    }
}