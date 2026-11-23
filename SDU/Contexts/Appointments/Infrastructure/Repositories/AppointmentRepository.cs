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

    public async Task<Appointment?> GetByIdAsync(Guid appointmentId)
    {
        return await _dbContext.Appointments.FindAsync(appointmentId);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public Task<List<Appointment>> GetWithFilterAsync(AppointmentFilter filter)
    {
        IQueryable<Appointment> query = _dbContext.Appointments.AsNoTracking();

        if (filter.CitizenId is not null)
            query = query.Where(a => a.CitizenId == filter.CitizenId);

        if (filter.ProfessionalId is not null)
            query = query.Where(a => a.ProfessionalId == filter.ProfessionalId);
        
        if (filter.EntityId is not null)
            query = query.Where(a => a.EntityId == filter.EntityId);
        
        if (filter.Status is not null)
            query = query.Where(a => a.Status == filter.Status);

        if (filter.Date is DateTime && filter.Mode is null)
            throw new ArgumentException("Selecione um modo de filtro por datas");
        
        if (filter.Date is DateTime date && filter.Mode is ComparisonMode mode)
        {
            if (mode == ComparisonMode.Before)
                query = query.Where(a => a.Date <= filter.Date);

            if (mode == ComparisonMode.Around)
            {
                DateTime from = date.AddMinutes(-30);
                DateTime to = date.AddMinutes(30);
                query = query.Where(
                    a => 
                    a.Date > from && a.Date < to
                    );
            }

            if (mode == ComparisonMode.After)
                query = query.Where(a => a.Date >= filter.Date);
        }
        
        return query.OrderBy(a => a.Date).ToListAsync();
    }
}