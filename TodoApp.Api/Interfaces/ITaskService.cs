public interface ITaskService
{
    Task<TaskItem> CreateAsync(CreateTaskDto dto);

    Task<List<TaskItem>> GetAllAsync(int taskListId);

    Task<TaskItem?> GetByIdAsync(int id);

    Task<TaskItem?> UpdateAsync(int id, UpdateTaskDto dto);

    Task<bool> DeleteAsync(int id);

    Task<TaskItem?> UpdateIsImportant(int id, bool newIsImportant);
    Task<TaskItem?> UpdateIsCompleted(int id, bool newIsCompleted);
}