namespace WebApplication1.DTO;

public class CredentialDTO
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public string? Username { get; set; }

    public string? PasswordHash { get; set; }

    public int? RoleId { get; set; }
}