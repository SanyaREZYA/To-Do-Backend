using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TaskListController : ControllerBase
{
    private readonly ITaskListService _taskListService;

    public TaskListController(ITaskListService taskListService)
    {
        _taskListService = taskListService;
    }

    [HttpPost]
    public async Task<ActionResult<TaskList>> Create(int userId, string name)
    {
        var createdTaskList = await _taskListService.CreateAsync(userId, name);
        return CreatedAtAction(nameof(GetById), new { id = createdTaskList.Id }, createdTaskList);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<TaskList>>> GetAll(int userId)
    {
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
    public async Task<ActionResult<TaskList>> Update(int id, string name)
    {
        var updatedTaskList = await _taskListService.UpdateAsync(id, name);
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