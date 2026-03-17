namespace Employee.Dtos.Auth
{
    public class NewUserRoleRequestDto
    {
        public string? UserEmail { get; set; }
        public string[]? Roles { get; set; }
    }
}
