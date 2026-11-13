using Microsoft.EntityFrameworkCore;
using SDU.Contexts.Appointments.Domain.Entities;
using SDU.Contexts.Appointments.Domain.IRepositories;
using SDU.Contexts.Appointments.Infrastructure.Persistance;

namespace SDU.Contexts.Appointments.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppointmentDbContext _dbContext;
    public AppointmentRepository(AppointmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Appointment appointment)
    {
        await _dbContext.Appointments.AddAsync(appointment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid appointmentId)
    {
        Appointment appointment =  await _dbContext.Appointments.FindAsync(appointmentId);
        _dbContext.Appointments.Remove(appointment);
        await _dbContext.SaveChangesAsync();
    }

    public Task<List<Appointment>> GetWithFilterAsync(AppointmentFilter filter)
    {
        IQueryable<Appointment> query = _dbContext.Appointments.AsNoTracking();

        if (filter.CitizenId is not null)
            query = query.Where(a => a.CitizenId == filter.CitizenId);

        if (filter.ProfessionalId is not null)
            query = query.Where(a => a.ProfessionId == filter.ProfessionalId);

        if (filter.Date is not null)
            query = query.Where(a => a.Date >= filter.Date);
        
        return query.OrderBy(a => a.Date).ToListAsync();
    }
}