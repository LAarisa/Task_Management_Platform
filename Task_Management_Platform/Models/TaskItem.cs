namespace Task_Management_Platform.Models
{
    public enum TaskStatus { ToDo, InProgress, Done }
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public int TenantId { get; set; }
        public int? AssignedToUserId { get; set; }

        public User AssignedToUser { get; set; }
        public Tenant Tenant { get; set; }
    }
}
