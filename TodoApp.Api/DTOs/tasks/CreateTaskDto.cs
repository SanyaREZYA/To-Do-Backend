using System.ComponentModel.DataAnnotations;

public class CreateTaskDto
{
    [Required(ErrorMessage = "TaskList is required")]
    public int TaskListId { get; set; }

    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsImportant { get; set; }

    public DateTime? DueDate { get; set; }
}