using System;
using System.Collections.Generic;

namespace WebApplication1.DB;

public partial class Role
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Credential> Credentials { get; set; } = new List<Credential>();
}
