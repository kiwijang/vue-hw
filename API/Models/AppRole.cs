using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class AppRole : IdentityRole<int>
    {
        public bool isActive { get; set; } = true;
    }
}