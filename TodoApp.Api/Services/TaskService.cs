using Microsoft.EntityFrameworkCore;
public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;

    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TaskItem> CreateAsync(CreateTaskDto dto)
    {
        var task = new TaskItem
        {
            TaskListId = dto.TaskListId,
            Title = dto.Title,
            Description = dto.Description,
            IsImportant = dto.IsImportant,
            DueDate = dto.DueDate
        };
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<List<TaskItem>> GetAllAsync(int taskListId)
    {
        return await _context.Tasks.Where(task => task.TaskListId == taskListId).ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks.FirstOrDefaultAsync(task => task.Id == id);
    }

    public async Task<TaskItem?> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return null;
        }

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.DueDate = dto.DueDate;

        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return false;
        }

        _context.Tasks.Remove(task);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<TaskItem?> UpdateIsImportant(int id, bool newIsImportant)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return null;
        }

        task.IsImportant = newIsImportant;

        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<TaskItem?> UpdateIsCompleted(int id, bool newIsCompleted)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return null;
        }

        task.IsCompleted = newIsCompleted;

        await _context.SaveChangesAsync();
        return task;
    }
}