using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class MemberVM
    {
        public int Id { get; set; }
        public string ChiName { get; set; }
        public string Gender { get; set; }
        public DateTime? Birth { get; set; }
        public string IdNumber { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string School { get; set; }
        public string Department { get; set; }
    }
}