using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskListController : ControllerBase
{
    private readonly ITaskListService _taskListService;

    public TaskListController(ITaskListService taskListService)
    {
        _taskListService = taskListService;
    }

    [HttpPost]
    public async Task<ActionResult<TaskList>> Create(TaskListDto dto)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }
        var createdTaskList = await _taskListService.CreateAsync(userId, dto);
        return CreatedAtAction(nameof(GetById), new { id = createdTaskList.Id }, createdTaskList);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<TaskList>>> GetAll()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }
        var taskLists = await _taskListService.GetAllAsync(userId);
        return Ok(taskLists);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskList>> GetById(int id)
    {
        var taskList = await _taskListService.GetByIdAsync(id);

        if (taskList == null)
        {
            return NotFound();
        }
        return Ok(taskList);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskList>> Update(int id, [FromBody] TaskListDto dto)
    {
        var updatedTaskList = await _taskListService.UpdateAsync(id, dto);
        if (updatedTaskList == null)
        {
            return NotFound();
        }
        return Ok(updatedTaskList);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _taskListService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}