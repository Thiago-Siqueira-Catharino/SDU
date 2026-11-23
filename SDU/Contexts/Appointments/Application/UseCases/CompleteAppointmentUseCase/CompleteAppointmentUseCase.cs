using System.Security.Authentication;
using SDU.Contexts.Appointments.Domain.Entities;
using SDU.Contexts.Appointments.Domain.IRepositories;

namespace SDU.Contexts.Appointments.Application.UseCases.CompleteAppointmentUseCase;

public class CompleteAppointmentUseCase
{
    private readonly IAppointmentRepository _appointmentRepository;
    public CompleteAppointmentUseCase(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
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
        
        appointment.Efetuar();
        await _appointmentRepository.SaveChangesAsync();
    }
}