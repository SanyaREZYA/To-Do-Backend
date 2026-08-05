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
            DueDate = dto.DueDate.HasValue
        ? DateTime.SpecifyKind(dto.DueDate.Value, DateTimeKind.Utc)
        : null
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

    public async Task<PagedResponseDto<TaskItem>> GetTasksAsync(
    GetTasksQueryDto query)
    {
        query.Page = Math.Max(query.Page, 1);
        query.PageSize = Math.Clamp(query.PageSize, 1, 100);

        IQueryable<TaskItem> tasksQuery = _context.Tasks
            .Where(task => task.TaskListId == query.TaskListId);

        tasksQuery = query.SortBy.ToLower() switch
        {
            "title" => query.SortDirection.ToLower() == "desc"
                ? tasksQuery
                    .OrderByDescending(task => task.Title)
                    .ThenBy(task => task.Id)
                : tasksQuery
                    .OrderBy(task => task.Title)
                    .ThenBy(task => task.Id),

            "duedate" => query.SortDirection.ToLower() == "desc"
                ? tasksQuery
                    .OrderBy(task => task.DueDate == null)
                    .ThenByDescending(task => task.DueDate)
                    .ThenBy(task => task.Title)
                    .ThenBy(task => task.Id)
                : tasksQuery
                    .OrderBy(task => task.DueDate == null)
                    .ThenBy(task => task.DueDate)
                    .ThenBy(task => task.Title)
                    .ThenBy(task => task.Id),

            _ => query.SortDirection.ToLower() == "desc"
                ? tasksQuery.OrderByDescending(task => task.Id)
                : tasksQuery.OrderBy(task => task.Id)
        };

        var totalCount = await tasksQuery.CountAsync();

        var skip = (query.Page - 1) * query.PageSize;

        var tasks = await tasksQuery
            .Skip(skip)
            .Take(query.PageSize)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(
            (double)totalCount / query.PageSize
        );

        return new PagedResponseDto<TaskItem>
        {
            Items = tasks,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
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
        task.DueDate = dto.DueDate.HasValue
        ? DateTime.SpecifyKind(dto.DueDate.Value, DateTimeKind.Utc)
        : null;

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

    public async Task<TaskItem?> UpdateIsImportant(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return null;
        }

        task.IsImportant = !task.IsImportant;

        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<TaskItem?> UpdateIsCompleted(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return null;
        }

        task.IsCompleted = !task.IsCompleted;

        await _context.SaveChangesAsync();
        return task;
    }
}