namespace Task_Management_Platform.Models
{
    public enum UserRole { Admin, Manager, User}
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public int TenantId { get; set; }

        public Tenant Tenant { get; set; }
        public ICollection<TaskItem> AssignedTasks { get; set; }

    }
}
