using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class UpdateUserVM
    {
        public string Email { get; set; }
        // 大頭貼
        public string? Img { get; set; }
        // 姓名
        public string? ChiName { get; set; }
        // 英文姓名
        public string? EngName { get; set; }
        // 手機
        public string? Phone { get; set; }
        // 性別
        public string? Gender { get; set; }
        // 生日
        public string? Birth { get; set; }
        // 地址
        public string? Address { get; set; }
        // 是否訂閱電子報
        public bool? IsSubscribe{ get; set; }
    }
}