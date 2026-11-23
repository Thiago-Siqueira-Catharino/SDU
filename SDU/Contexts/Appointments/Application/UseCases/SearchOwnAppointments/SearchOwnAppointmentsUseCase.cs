using SDU.Contexts.Appointments.Domain.Entities;
using SDU.Contexts.Appointments.Domain.IRepositories;

namespace SDU.Contexts.Appointments.Application.UseCases.SearchOwnAppointments;

public class SearchOwnAppointmentsUseCase
{
    private readonly IAppointmentRepository _appointmentRepository;
    public SearchOwnAppointmentsUseCase(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<List<Appointment>> Run(
        SearchOwnAppointmentsDto dto,
        Guid id,
        string role
    )
    {
        AppointmentFilter filter = new AppointmentFilter
        (
            Mode: dto.Mode,
            Status: dto.Status
        );

        if (dto.Mode is not null)
            filter = filter with { Date = DateTime.UtcNow };
            
        if (role == "Citizen")
            filter = filter with {CitizenId = id};
        
        else if (role == "Professional")
            filter = filter with {ProfessionalId = id};

        else if (role == "Entity")
            filter = filter with { EntityId = id };

        else
            throw new UnauthorizedAccessException("Você não tem autorização para realizar essa busca");

        return await _appointmentRepository.GetWithFilterAsync(filter);
    }
}