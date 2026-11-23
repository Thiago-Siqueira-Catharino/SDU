using SDU.Contexts.Appointments.Domain.Entities;
using SDU.Contexts.Appointments.Domain.IRepositories;

namespace SDU.Contexts.Appointments.Application.UseCases.SearchAppointmentsUseCase;

public class SearchAppointmentsUseCase
{
    private readonly IAppointmentRepository _appointmentRepository;
    public SearchAppointmentsUseCase(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<List<Appointment>> Run(AppointmentFilter filter)
    {
        bool semPaciente = filter.CitizenId is null;
        bool semProfissional = filter.ProfessionalId is null;
        bool semData = filter.Date is null;

        if (semPaciente && semProfissional && semData)
        {
            throw new ArgumentException(
                "Use ao menos uma das opções disponíveis no filtro: Id do paciente, do profissional e/ou data"
            );
        }

        return await _appointmentRepository.GetWithFilterAsync(filter);
    }
}