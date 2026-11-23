using SDU.Contexts.Appointments.Domain.Entities;
using SDU.Contexts.Appointments.Domain.IRepositories;

namespace SDU.Contexts.Appointments.Application.UseCases.NewAppointmentUseCase;

public class NewAppointmentUseCase
{
    private readonly IAppointmentRepository _appointmentRepository;
    public NewAppointmentUseCase(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
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

    public async Task Run(NewAppointmentDto dto)
    {
        Guid patientId = dto.PatientId;
        if (patientId == Guid.Empty)
            throw new ArgumentException($"{nameof(dto.PatientId)} cannot be empty");

        Guid professionalId = dto.ProfessionalId;
        if (professionalId == Guid.Empty)
            throw new ArgumentException($"{nameof(dto.ProfessionalId)} cannot be empty");

        Guid entityId = dto.EntityId;
        if (entityId == Guid.Empty)
            throw new ArgumentException($"{nameof(dto.EntityId)} cannot be empty");

        DateTime date = dto.Date;
        if (date == default(DateTime))
            throw new ArgumentException($"{nameof(dto.Date)} cannot be empty");

        bool citizenBusy = await CitizenHasAppointmentAround(patientId, date);
        if (citizenBusy)
            throw new InvalidOperationException("O paciente já tem uma consulta próxima a esse horário");

        bool professionalBusy = await ProfessionalHasAppointmentAround(professionalId, date);
        if (professionalBusy)
            throw new InvalidOperationException("O profissional já tem uma consulta próxima a esse horário");

        Appointment toSave = new Appointment(patientId, professionalId, entityId, date);
        await _appointmentRepository.AddAsync(toSave);
    }
}