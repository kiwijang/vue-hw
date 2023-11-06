using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace API.Services
{
    public interface IMemberService
    {
        Task<MemberVM?> GetMemberByUserId(string id);
        Task<IQueryable<MemberVM?>> GetMembers();
        Task<bool> UpdateMember(MemberVM model);
    }

    public class MemberService : IMemberService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly VueHwDbContext _db;

        public MemberService(UserManager<AppUser> userManager, VueHwDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<IQueryable<MemberVM?>> GetMembers()
        {
            return _db.AppUsers.Select(x => new MemberVM()
            {
                Id = x.Id,
                ChiName = x.ChiName,
                Gender = x.Gender,
                Birth = x.BirthDay,
                IdNumber = "",
                Email = x.Email,
                Phone = x.PhoneNumber,
                Address = x.Address,
                School = x.SchoolName,
                Department = x.Department,
            });
        }

        public async Task<MemberVM?> GetMemberByUserId(string id)
        {
            AppUser? user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return null;
            }

            return new MemberVM()
            {
                Id = user.Id,
                ChiName = user.ChiName,
                Gender = user.Gender,
                Birth = user.BirthDay,
                IdNumber = user.IdNumber,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Address = user.Address,
                School = user.SchoolName,
                Department = user.Department,
            };
        }

        public async Task<bool> UpdateMember(MemberVM model)
        {
            AppUser? user = await _userManager.FindByIdAsync(model.Id.ToString());

            if (user == null)
            {
                return false;
            }

            user.ChiName = model.ChiName;
            user.Gender = model.Gender;
            user.BirthDay = model.Birth;
            user.IdNumber = model.IdNumber;
            user.Email = model.Email;
            user.PhoneNumber = model.Phone;
            user.Address = model.Address;
            user.SchoolName = model.School;
            user.Department = model.Department;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}