using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class EventVM
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Address { get; set; }

        public decimal Cost { get; set; }

        public virtual IList<EventjoinusersVM>? EventjoinusersName { get; set; }
    }
}