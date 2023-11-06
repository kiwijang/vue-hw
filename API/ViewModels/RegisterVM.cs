using System.ComponentModel.DataAnnotations;

namespace API.ViewModels
{
    public class RegisterVM
    {
        /// <summary>
        /// Email
        /// </summary>
        /// <value></value>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        /// <value></value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 手機：(不可重複)
        /// </summary>
        /// <value></value>
        [Required]
        public string Phone { get; set; }

        /// <summary>
        /// (*6-30碼字元須包含最少1個大寫字母、最少1個小寫字母、 最少1個數字，不可有其他符號)
        /// </summary>
        /// <value></value>
        [Required]
        public string Pwd { get; set; }

        [Required]
        public string RecaptchaToken { get; set; }

        public Boolean RememberMe { get; set; }

        public RegisterVM()
        {

        }
    }
}