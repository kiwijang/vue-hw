using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Models;

public partial class AppUser: IdentityUser<int>
{
    public DateTime CreateTime { get; set; }

    public string? Img { get; set; } = null!;

    public string ChiName { get; set; } = null!;

    public string? EngName { get; set; } = null!;

    public string? Gender { get; set; } = null!;

    public DateTime? BirthDay { get; set; }

    public string? Address { get; set; }

    public bool? IsSubscribe { get; set; }

    public string? SchoolName { get; set; }

    public string? Department { get; set; }
    public string? IdNumber { get; set; }
}
