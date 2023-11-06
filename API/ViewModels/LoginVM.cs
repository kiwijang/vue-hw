using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class LoginVM
    {
        public string Email { get; set; }
        public string Pwd { get; set; }
        public bool RememberMe { get; set; }
        public string RecaptchaToken { get; set; }
    }
}