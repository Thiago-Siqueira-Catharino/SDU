using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SDU.Contexts.Appointments.Application.UseCases.CancelAppointmentUseCase;
using SDU.Contexts.Appointments.Application.UseCases.NewAppointmentUseCase;
using SDU.Contexts.Appointments.Application.UseCases.CompleteAppointmentUseCase;
using SDU.Contexts.Appointments.Application.UseCases.MissedAppointment;
using SDU.Contexts.Appointments.Application.UseCases.SearchAppointmentsUseCase;
using SDU.Contexts.Appointments.Application.UseCases.SearchOwnAppointments;
using SDU.Contexts.Appointments.Domain.IRepositories;

namespace SDU.Contexts.Appointments.Presentation;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController(
    NewAppointmentUseCase _newAppointmentUseCase,
    CancelAppointmentUseCase _cancelAppointmentUseCase,
    CompleteAppointmentUseCase _completeAppointmentUseCase,
    MissedAppointmentUseCase _missedAppointmentUseCase,
    SearchAppointmentsUseCase _searchAppointmentUseCase,
    SearchOwnAppointmentsUseCase _searchOwnAppointmentUseCase
    ) : ControllerBase
{
    [HttpPost("new")]
    [Authorize(Roles = "Professional")]
    public async Task<IActionResult> NewAppointment(NewAppointmentDto dto)
    {
        var userIdStr = User.FindAll(ClaimTypes.NameIdentifier)
            .Select(c => c.Value)
            .FirstOrDefault(v => Guid.TryParse(v, out _));

        if (!Guid.TryParse(userIdStr, out Guid professionalId))
            return Unauthorized();
        
        try
        {
            await _newAppointmentUseCase.Run(dto, professionalId);
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

    [HttpPost("complete")]
    [Authorize(Roles = "Professional, Entity")]
    public async Task<IActionResult> CompleteAppointment(Guid toCompleteId)
    {
        var userIdStr = User.FindAll(ClaimTypes.NameIdentifier)
            .Select(c => c.Value)
            .FirstOrDefault(v => Guid.TryParse(v, out _));
        
        if (!Guid.TryParse(userIdStr, out Guid userId))
            return Unauthorized();

        try
        {
            await _completeAppointmentUseCase.Run(toCompleteId, userId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("missed")]
    [Authorize(Roles = "Professional, Entity")]
    public async Task<IActionResult> MarkAsMissedAppointment(Guid toMarkAsMissedId)
    {
        var userIdStr = User.FindAll(ClaimTypes.NameIdentifier)
            .Select(c => c.Value)
            .FirstOrDefault(v => Guid.TryParse(v, out _));
        
        if (!Guid.TryParse(userIdStr, out Guid userId))
            return Unauthorized();
        
        try
        {
            await _missedAppointmentUseCase.Run(toMarkAsMissedId, userId);
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

    [HttpPost("search/own")]
    [Authorize]
    public async Task<IActionResult> SearchOwnAppointments([FromBody] SearchOwnAppointmentsDto dto)
    {
        var userIdStr = User.FindAll(ClaimTypes.NameIdentifier)
            .Select(c => c.Value)
            .FirstOrDefault(v => Guid.TryParse(v, out _));
        
        if (!Guid.TryParse(userIdStr, out Guid userId))
            return Unauthorized();
        
        string? role = User.FindFirstValue(ClaimTypes.Role);
        if (string.IsNullOrEmpty(role))
            return Unauthorized();

        try
        {
            var appointments = await _searchOwnAppointmentUseCase.Run(dto, userId, role);
            return Ok(appointments);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}