using EclipseApi.Application.Dtos;
using EclipseApi.Application.Queries;
using EclipseApi.Controllers.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtém o relatório de desempenho das tarefas do usuário
    /// </summary>
    [HttpGet("performance/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PerformanceReportDto>))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPerformanceReport(
        Guid userId,
        [FromHeader(Name = "Role")] string role)
    {
        if (string.IsNullOrEmpty(role) || role.ToLower() != "gerente")
        {
            return Forbid("Access Denied: Only managers can access this resource.");
        }

        var query = new GetPerformanceReportQuery{UserId = userId};

        var result = await _mediator.Send(query);
        return ResponseHelper.SuccessResponse(result);
    }
}
