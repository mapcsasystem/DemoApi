using System;
using System.Collections.Generic;

namespace Demo.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public short Age { get; set; }
}
