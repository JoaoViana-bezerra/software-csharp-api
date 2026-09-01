using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Extensions;
using TaskFlow.Application.DTOs.Tasks;
using TaskFlow.Application.Services;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/tasks")]
public sealed class TasksController : ControllerBase
{
    private readonly TaskService _taskService;

    public TasksController(TaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyCollection<TaskResponse>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var tasks = await _taskService.GetAllAsync(
            userId,
            cancellationToken
        );

        return Ok(tasks);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(TaskResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var task = await _taskService.GetByIdAsync(
            userId,
            id,
            cancellationToken
        );

        return Ok(task);
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(TaskResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTaskRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var task = await _taskService.CreateAsync(
            userId,
            request,
            cancellationToken
        );

        return CreatedAtAction(
            nameof(GetById),
            new { id = task.Id },
            task
        );
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(
        typeof(TaskResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateTaskRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var task = await _taskService.UpdateAsync(
            userId,
            id,
            request,
            cancellationToken
        );

        return Ok(task);
    }

    [HttpPatch("{id:guid}/start")]
    [ProducesResponseType(
        typeof(TaskResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Start(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var task = await _taskService.StartAsync(
            userId,
            id,
            cancellationToken
        );

        return Ok(task);
    }

    [HttpPatch("{id:guid}/complete")]
    [ProducesResponseType(
        typeof(TaskResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Complete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var task = await _taskService.CompleteAsync(
            userId,
            id,
            cancellationToken
        );

        return Ok(task);
    }

    [HttpPatch("{id:guid}/reopen")]
    [ProducesResponseType(
        typeof(TaskResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reopen(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var task = await _taskService.ReopenAsync(
            userId,
            id,
            cancellationToken
        );

        return Ok(task);
    }

    [HttpPatch("{id:guid}/cancel")]
    [ProducesResponseType(
        typeof(TaskResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var task = await _taskService.CancelAsync(
            userId,
            id,
            cancellationToken
        );

        return Ok(task);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(
        StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        await _taskService.DeleteAsync(
            userId,
            id,
            cancellationToken
        );

        return NoContent();
    }
}