using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SDU.Contexts.Files.Application.UseCases.DiagnosticUpload;
using SDU.Contexts.Files.Application.UseCases.DownloadUseCase;
using SDU.Contexts.Files.Application.UseCases.ExamUpload;
using SDU.Contexts.Files.Application.UseCases.GetExamFilesByCivilianId;

namespace SDU.Contexts.Files.Presentation;

[ApiController]
[Route("api/[controller]")]
public class FileController(
    ExamUploadUseCase _examUploadUseCase,
    DiagnosticUploadUseCase _diagnosticUploadUseCase,
    DownloadUseCase _downloadUseCase,
    GetCitizenFilesUseCase _getFilesUseCase
    ) : ControllerBase
{
    
    [HttpPost("upload/exam")]
    [Consumes("multipart/form-data")]
    [Authorize(Roles = "Entity, Professional")]
    public async Task<IActionResult> ReceiveExamFile([FromForm] string civilianId, string examType, IFormFile file)
    {
        var userId = User.FindAll(ClaimTypes.NameIdentifier)
            .Select(c => c.Value)
            .FirstOrDefault(v => Guid.TryParse(v, out _));

        var uploadDto = new ExamUploadDto
        {
            ProfessionalId = userId,
            CivilianId = civilianId,
            ExamType = examType,
            File = file
        };
            
        await _examUploadUseCase.Run(uploadDto);
        return Ok();
    }
    
    [HttpPost("upload/diagnostic")]
    [Consumes("multipart/form-data")]
    [Authorize(Roles = "Entity, Professional")]
    public async Task<IActionResult> ReceiveDiagnosticFile([FromForm] string civilianId, string cid, IFormFile file)
    {
        var userId = User.FindAll(ClaimTypes.NameIdentifier)
            .Select(c => c.Value)
            .FirstOrDefault(v => Guid.TryParse(v, out _));

        var uploadDto = new DiagnosticUploadDto
        {
            ProfessionalId = userId,
            CivilianId = civilianId,
            Cid = cid,
            File = file
        };
            
        await _diagnosticUploadUseCase.Run(uploadDto);
        return Ok();
    }

    [HttpPost("download")]
    [Authorize(Roles = "Entity, Professional, Citizen")]
    public async Task<IActionResult> SendFile([FromForm] DownloadDto downloadDto)
    {
        try
        {
            var result = await _downloadUseCase.Run(downloadDto);
            return File(result.FileStream, result.ContentType, result.FileName);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("getbyid")]
    [Authorize(Roles = "Professional")]
    public async Task<IActionResult> GetById([FromQuery] string civilianId)
    {
        try
        {
            var result = await _getFilesUseCase.Run(civilianId);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet("getownbyid")]
    [Authorize(Roles = "Citizen")]
    public async Task<IActionResult> GetOwnById()
    {
        var userId = User.FindAll(ClaimTypes.NameIdentifier)
            .Select(c => c.Value)
            .FirstOrDefault(v => Guid.TryParse(v, out _));
        
        if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
            return Unauthorized();
        
        string civilianId = userId;
        
        try
        {
            var result = await _getFilesUseCase.Run(civilianId);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}