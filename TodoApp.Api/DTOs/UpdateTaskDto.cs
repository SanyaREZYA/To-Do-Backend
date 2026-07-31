using System.ComponentModel.DataAnnotations;

public class UpdateTaskDto
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
}