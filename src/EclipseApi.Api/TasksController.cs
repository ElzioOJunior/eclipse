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
using EclipseApi.Api.Helpers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lista todas as tarefas de um projeto
    /// </summary>
    /// <param name="projectId">Id do projeto</param>
    /// <returns>Lista de tarefas</returns>
    [HttpGet("{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TaskDto>))]
    public async Task<IActionResult> GetTasks(Guid projectId)
    {
        var query = new GetTasksByProjectQuery { ProjectId = projectId };
        var result = await _mediator.Send(query);

        return ResponseHelper.SuccessResponse(result);
    }

    /// <summary>
    /// Cria uma nova tarefa
    /// </summary>
    /// <param name="command">Dados da nova tarefa</param>
    /// <returns>Tarefa criada</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TaskDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
    {

        var result = await _mediator.Send(command);

        return ResponseHelper.SuccessResponse(result);
    }

    /// <summary>
    /// Atualiza uma tarefa existente
    /// </summary>
    /// <param name="id">Id da tarefa</param>
    /// <param name="command">Dados atualizados da tarefa</param>
    /// <returns>Tarefa atualizada</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateTask([FromRoute] Guid id, [FromBody] UpdateTaskDto dto)
    {
        var command = new UpdateTaskCommand(id, dto.Title, dto.Description, dto.UpdatedByUserId, dto.Status, dto.UserId);
        var result = await _mediator.Send(command);

        return ResponseHelper.SuccessResponse(result);
    }

    /// <summary>
    /// Atualiza uma tarefa existente
    /// </summary>
    /// <param name="taskId">Id da tarefa</param>
    /// <param name="command">Dados atualizados da tarefa</param>
    /// <returns>Tarefa atualizada</returns>
    [HttpPost("{id}/comments")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommentDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddComment(Guid id, [FromBody] AddCommentDto dto)
    {
        var command = new AddCommentCommand(id, dto.Content, dto.UserId);
        var result = await _mediator.Send(command);
        return ResponseHelper.SuccessResponse(result);
    }

}
