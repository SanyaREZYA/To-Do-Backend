using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create(CreateTaskDto dto)
    {
        var createdTask = await _taskService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
    }

    [HttpGet("list/{taskListId}")]
    public async Task<ActionResult<List<TaskItem>>> GetAll(int taskListId)
    {
        var tasks = await _taskService.GetAllAsync(taskListId);

        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetById(int id)
    {
        var task = await _taskService.GetByIdAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        return Ok(task);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskItem>> Update(int id, UpdateTaskDto dto)
    {
        var updatedTask = await _taskService.UpdateAsync(id, dto);

        if (updatedTask == null)
        {
            return NotFound();
        }

        return Ok(updatedTask);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _taskService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPatch("{id}/important")]
    public async Task<ActionResult<TaskItem>> UpdateIsImportant(int id, [FromBody] bool isImportant)
    {
        var updatedTask = await _taskService.UpdateIsImportant(id, isImportant);

        if (updatedTask == null)
        {
            return NotFound();
        }

        return Ok(updatedTask);
    }

    [HttpPatch("{id}/completed")]
    public async Task<ActionResult<TaskItem>> UpdateIsCompleted(int id, [FromBody] bool isCompleted)
    {
        var updatedTask = await _taskService.UpdateIsCompleted(id, isCompleted);

        if (updatedTask == null)
        {
            return NotFound();
        }

        return Ok(updatedTask);
    }
}