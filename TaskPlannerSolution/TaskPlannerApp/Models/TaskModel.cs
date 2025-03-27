using System.ComponentModel.DataAnnotations;

namespace TaskPlannerApp.Models
{
    public class TaskModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? TaskName { get; set; }
        [Required]
        public string? TaskAuthor { get; set; }
        public AppTaskStatus Status { get; set; }
    }


    public enum AppTaskStatus
    {
        ToDo = 0,
        InProgress = 1,
        Done = 2
    }
}
