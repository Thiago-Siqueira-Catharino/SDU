using SDU.Contexts.Appointments.Domain.Entities;

namespace SDU.Contexts.Appointments.Domain.IRepositories;

public interface IAppointmentRepository
{
    Task AddAsync(Appointment appointment);
    Task RemoveAsync(Guid appointmentId);
    Task<Appointment?> GetByIdAsync(Guid appointmentId);
    Task SaveChangesAsync();
    Task<List<Appointment>> GetWithFilterAsync(AppointmentFilter citizenId);
}