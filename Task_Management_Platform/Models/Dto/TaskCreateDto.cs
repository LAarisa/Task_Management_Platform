namespace Task_Management_Platform.Models.Dto
{
    public class TaskCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "ToDo";
        public int AssignedToUserId { get; set; }
    }
}
