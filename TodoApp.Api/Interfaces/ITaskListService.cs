public interface ITaskListService
{
    Task<TaskList> CreateAsync(int userId, TaskListDto dto);

    Task<List<TaskList>> GetAllAsync(int userId);

    Task<TaskList?> GetByIdAsync(int id);

    Task<TaskList?> UpdateAsync(int id, TaskListDto dto);

    Task<bool> DeleteAsync(int id);
}