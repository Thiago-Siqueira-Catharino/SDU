using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SDU.Contexts.Appointments.Application.UseCases.CancelAppointmentUseCase;
using SDU.Contexts.Appointments.Application.UseCases.NewAppointmentUseCase;

namespace SDU.Contexts.Appointments.Presentation;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController(
    NewAppointmentUseCase _newAppointmentUseCase,
    CancelAppointmentUseCase _cancelAppointmentUseCase
    ) : ControllerBase
{
    [HttpPost("new")]
    [Authorize(Roles = "Professional")]
    public async Task<IActionResult> NewAppointment(NewAppointmentDto dto)
    {
        await _newAppointmentUseCase.Run(dto);
        return Ok();
    }

    [HttpPost("cancel")]
    [Authorize]
    public async Task<IActionResult> CancelAppointment(Guid toCancelId)
    {
        var userIdStr = User.FindAll(ClaimTypes.NameIdentifier)
            .Select(c => c.Value)
            .FirstOrDefault(v => Guid.TryParse(v, out _));

        if (!Guid.TryParse(userIdStr, out Guid userId))
            return Unauthorized();
        
        await _cancelAppointmentUseCase.Run(toCancelId, userId);
        return Ok();
    }
}