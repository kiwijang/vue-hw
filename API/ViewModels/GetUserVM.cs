namespace API.ViewModels
{
    public class GetUserVM
    {
        // 	Email
        public string Email { get; set; }

        // 	加入會員時間
        public DateTime CreateTime { get; set; }

        // 	大頭貼
        public string Img { get; set; }
        // 	中文姓名
        public string ChiName { get; set; }
        // 	英文姓名
        public string EngName { get; set; }
        // 	手機
        public string Phone { get; set; }
        // 	性別
        public string Gender { get; set; }
        // 	生日
        public DateTime BirthDay { get; set; }
        // 	地址
        public string Address { get; set; }
        // 	是否訂閱電子報
        public Boolean isSubscribe { get; set; }
    }
}