using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SDU.Contexts.Appointments.Application.UseCases.NewAppointmentUseCase;

namespace SDU.Contexts.Appointments.Presentation;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController(
    NewAppointmentUseCase _newAppointmentUseCase
    ) : ControllerBase
{
    [HttpPost("new")]
    [Authorize(Roles = "Professional")]
    public async Task<IActionResult> NewAppointment(NewAppointmentDto dto)
    {
        await _newAppointmentUseCase.Run(dto);
        return Ok();
    }
}