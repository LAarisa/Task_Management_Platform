namespace Task_Management_Platform.Models.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
