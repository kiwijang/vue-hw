using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace API.Services
{
    public interface IUserService
    {
        Task<GetUserVM?> GetUser(string email);
        Task<bool> UpdateUser(UpdateUserVM model);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetUserVM?> GetUser(string email)
        {

            AppUser? user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            return new GetUserVM
            {
                Email = user.Email,
                CreateTime = DateTime.Now, // 設定加入會員時間為當前時間，您可以根據需求更改此值
                Img = user.Img, // 填入大頭貼的URL或路徑
                ChiName = user.ChiName,
                EngName = user.EngName, // 填入英文姓名
                Phone = user.PhoneNumber, // 填入手機號碼
                Gender = user.Gender, // 填入性別，例如 "男" 或 "女"
                BirthDay = user.BirthDay ?? DateTime.Now, // 設定生日為當前時間，您可以根據需求更改此值
                Address = user.Address, // 填入地址
                isSubscribe = user.IsSubscribe ?? false// 預設為未訂閱電子報，您可以根據需求更改此值
            };
        }

        public async Task<bool> UpdateUser(UpdateUserVM model)
        {

            AppUser? user = await _userManager.FindByEmailAsync(model.Email);
            
            if (user == null)
            {
                return false;
            }

            user.Img = model.Img;
            user.ChiName = model.ChiName;
            user.EngName = model.EngName; // 填入英文姓名
            user.PhoneNumber = model.Phone; // 填入手機號碼
            user.Gender = model.Gender; // 填入性別，例如 "男" 或 "女"
            user.BirthDay = model.Birth is null ? null : DateTime.Parse(model.Birth); // 設定生日為當前時間，您可以根據需求更改此值
            user.Address = model.Address; // 填入地址
            user.IsSubscribe = model.IsSubscribe; // 預設為未訂閱電子報，您可以根據需求更改此值

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}