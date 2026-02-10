using System;
using System.Collections.Generic;

namespace WebApplication1.DB;

public partial class Credential 
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public string? Username { get; set; }

    public string? PasswordHash { get; set; }

    public int? RoleId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Role? Role { get; set; }
}
