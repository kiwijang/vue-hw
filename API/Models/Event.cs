using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Event
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string Address { get; set; }

    public decimal Cost { get; set; }

    public virtual ICollection<Eventjoinuser> Eventjoinusers { get; set; } = new List<Eventjoinuser>();
}
