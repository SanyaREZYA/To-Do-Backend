using System.ComponentModel.DataAnnotations;

public class GetTasksQueryDto
{
    [Required(ErrorMessage = "TaskList is required")]
    public int TaskListId { get; set; }

    [Required(ErrorMessage = "Page number is required")]
    public int Page { get; set; } = 1;

    [Required(ErrorMessage = "Page size is required")]
    public int PageSize { get; set; } = 10;

    public string SortBy { get; set; } = "id";

    public string SortDirection { get; set; } = "asc";

    public string? Search { get; set; }
}