using EclipseApi.Application.Commands;
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
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lista todos os projetos de um usuário
    /// </summary>
    /// <param name="userId">Id do usuário</param>
    /// <returns>Lista de projetos</returns>
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProjectDto>))]
    public async Task<IActionResult> GetProjects(Guid userId)
    {
        var query = new GetProjectsQuery { UserId = userId };
        var result = await _mediator.Send(query);

        return ResponseHelper.SuccessResponse(result);
    }

    /// <summary>
    /// Cria um novo projeto
    /// </summary>
    /// <param name="command">Dados do novo projeto</param>
    /// <returns>Projeto criado</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProjectDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command)
    {

        var result = await _mediator.Send(command);

        return ResponseHelper.SuccessResponse(result);
    }

    /// <summary>
    /// Remove um projeto existente
    /// </summary>
    /// <param name="id">Id da projeto</param>
    /// <returns>Projeto removido</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskDto))]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        var command = new DeleteProjectCommand { Id = id };
        var result = await _mediator.Send(command);

        return ResponseHelper.SuccessResponse(result);
    }
}
