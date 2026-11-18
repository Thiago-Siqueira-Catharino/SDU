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
    
    public async Task Run(NewAppointmentDto dto)
    {
        Guid patient_Id = dto.PatientId;
        if (patient_Id ==  Guid.Empty || patient_Id == null)
            throw new ArgumentException($"{nameof(dto.PatientId)} cannot be null or empty");
        
        Guid profession_Id = dto.ProfessionId;
        if (profession_Id ==  Guid.Empty || profession_Id == null)
            throw new ArgumentException($"{nameof(dto.ProfessionId)} cannot be null or empty");
        
        Guid entity_Id = dto.EntityId;
        if (entity_Id ==  Guid.Empty || entity_Id == null)
            throw new ArgumentException($"{nameof(dto.EntityId)} cannot be null or empty");
        
        DateTime date = dto.Date;
        if (date == default(DateTime) || date == null)
            throw new ArgumentException($"{nameof(date)} cannot be null or empty");
        
        Appointment toSave = new Appointment(patient_Id, profession_Id, entity_Id, dto.Date);
        await _appointmentRepository.AddAsync(toSave);
    }
}