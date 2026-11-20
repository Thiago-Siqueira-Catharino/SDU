using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SDU.Contexts.Appointments.Application.UseCases.CancelAppointmentUseCase;
using SDU.Contexts.Appointments.Application.UseCases.NewAppointmentUseCase;
using SDU.Contexts.Appointments.Application.UseCases.SearchAppointmentsUseCase;
using SDU.Contexts.Appointments.Domain.IRepositories;

namespace SDU.Contexts.Appointments.Presentation;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController(
    NewAppointmentUseCase _newAppointmentUseCase,
    CancelAppointmentUseCase _cancelAppointmentUseCase,
    SearchAppointmentsUseCase _searchAppointmentUseCase
    ) : ControllerBase
{
    [HttpPost("new")]
    [Authorize(Roles = "Professional")]
    public async Task<IActionResult> NewAppointment(NewAppointmentDto dto)
    {
        try
        {
            await _newAppointmentUseCase.Run(dto);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
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

        try
        {
            await _cancelAppointmentUseCase.Run(toCancelId, userId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }

    [HttpPost("search")]
    [Authorize]
    public async Task<IActionResult> SearchAppointments([FromBody] AppointmentFilter filter)
    {
        try
        {
            return Ok(await _searchAppointmentUseCase.Run(filter));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
}