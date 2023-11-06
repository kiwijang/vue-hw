using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Eventjoinuser
{
    public Guid EventId { get; set; }

    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;
}
