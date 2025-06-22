namespace Task_Management_Platform.Models.Dto
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AssignedToUsername { get; set; }
        public string Status { get; set; }
    }
}
