public interface ITaskService
{
    Task<TaskItem> CreateAsync(CreateTaskDto dto);

    Task<List<TaskItem>> GetAllAsync(int taskListId);

    Task<TaskItem?> GetByIdAsync(int id);

    Task<PagedResponseDto<TaskItem>> GetTasksAsync(GetTasksQueryDto query);

    Task<TaskItem?> UpdateAsync(int id, UpdateTaskDto dto);

    Task<bool> DeleteAsync(int id);

    Task<TaskItem?> UpdateIsImportant(int id);
    Task<TaskItem?> UpdateIsCompleted(int id);
}