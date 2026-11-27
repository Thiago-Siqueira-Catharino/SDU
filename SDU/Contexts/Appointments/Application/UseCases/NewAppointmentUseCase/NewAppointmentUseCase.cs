using SDU.Contexts.Appointments.Domain.Entities;
using SDU.Contexts.Appointments.Domain.IRepositories;
using SDU.Contexts.Auth.Contracts;

namespace SDU.Contexts.Appointments.Application.UseCases.NewAppointmentUseCase;

public class NewAppointmentUseCase
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IAuthModuleApi _authModuleApi;
    public NewAppointmentUseCase(IAppointmentRepository appointmentRepository, IAuthModuleApi authModuleApi)
    {
        _appointmentRepository = appointmentRepository;
        _authModuleApi = authModuleApi;
    }

    private async Task<bool> CitizenHasAppointmentAround(Guid citizenId, DateTime date)
    {
        AppointmentFilter filter = new AppointmentFilter(
            CitizenId: citizenId,
            Date: date,
            Mode: ComparisonMode.Around
        );

        List<Appointment> found = await _appointmentRepository.GetWithFilterAsync(filter);
        return found.Count > 0;
    }

    private async Task<bool> ProfessionalHasAppointmentAround(Guid professionalId, DateTime date)
    {
        AppointmentFilter filter = new AppointmentFilter(
            ProfessionalId: professionalId,
            Date: date,
            Mode: ComparisonMode.Around
        );

        List<Appointment> found = await _appointmentRepository.GetWithFilterAsync(filter);
        return found.Count > 0;
    }

    public async Task Run(NewAppointmentDto dto, Guid professionalId)
    {
        Guid patientId = dto.PatientId;
        if (patientId == Guid.Empty)
            throw new ArgumentException($"{nameof(dto.PatientId)} cannot be empty");
        
        if (professionalId == Guid.Empty)
            throw new ArgumentException($"{nameof(professionalId)} cannot be empty");

        Guid? entityId = await _authModuleApi.GetEntityIdByProfessionalIdAsync(professionalId.ToString());
        if (entityId == Guid.Empty)
            throw new ArgumentException($"{nameof(entityId)} cannot be empty");

        DateTime date = dto.Date;
        if (date == default(DateTime))
            throw new ArgumentException($"{nameof(dto.Date)} cannot be empty");

        bool citizenBusy = await CitizenHasAppointmentAround(patientId, date);
        if (citizenBusy)
            throw new InvalidOperationException("O paciente já tem uma consulta próxima a esse horário");

        bool professionalBusy = await ProfessionalHasAppointmentAround(professionalId, date);
        if (professionalBusy)
            throw new InvalidOperationException("O profissional já tem uma consulta próxima a esse horário");

        Appointment toSave = new Appointment(patientId, professionalId, entityId.Value, date);
        await _appointmentRepository.AddAsync(toSave);
    }
}