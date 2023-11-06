using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class AddEventJoinUserVM
    {
        public Guid EventId { get; set; }
        public List<EventjoinusersVM> JoinUsers { get; set; }
    }
}