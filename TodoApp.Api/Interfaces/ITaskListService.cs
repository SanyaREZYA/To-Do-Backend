public interface ITaskListService
{
    Task<TaskList> CreateAsync(int userId, string name);

    Task<List<TaskList>> GetAllAsync(int userId);

    Task<TaskList?> GetByIdAsync(int id);

    Task<TaskList?> UpdateAsync(int id, string name);

    Task<bool> DeleteAsync(int id);
}