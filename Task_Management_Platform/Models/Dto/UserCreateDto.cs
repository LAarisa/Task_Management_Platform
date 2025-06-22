namespace Task_Management_Platform.Models.Dto
{
    public class UserCreateDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
