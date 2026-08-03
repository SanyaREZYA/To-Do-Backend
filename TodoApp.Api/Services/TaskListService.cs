using Microsoft.EntityFrameworkCore;

public class TaskListService : ITaskListService
{
    private readonly ApplicationDbContext _context;

    public TaskListService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TaskList> CreateAsync(int userId, TaskListDto dto)
    {
        var taskList = new TaskList
        {
            Name = dto.Name,
            UserId = userId
        };
        await _context.TaskLists.AddAsync(taskList);
        await _context.SaveChangesAsync();
        return taskList;
    }

    public async Task<List<TaskList>> GetAllAsync(int userId)
    {
        return await _context.TaskLists.Where(taskList => taskList.UserId == userId).ToListAsync();
    }

    public async Task<TaskList?> GetByIdAsync(int id)
    {
        return await _context.TaskLists.FirstOrDefaultAsync(taskList => taskList.Id == id);
    }

    public async Task<TaskList?> UpdateAsync(int id, TaskListDto dto)
    {
        var taskList = await _context.TaskLists.FindAsync(id);
        if (taskList == null)
        {
            return null;
        }

        taskList.Name = dto.Name;

        await _context.SaveChangesAsync();
        return taskList;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var taskList = await _context.TaskLists.FindAsync(id);
        if (taskList == null)
        {
            return false;
        }

        _context.TaskLists.Remove(taskList);

        await _context.SaveChangesAsync();
        return true;
    }
}