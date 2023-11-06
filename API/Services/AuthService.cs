
using System.Text;
using API.ViewModels;
using API.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace API.Services
{
    public interface IAuthService
    {
        public Task<Result> Register(RegisterVM model);
        public Task<Result> RegisterAdmin(RegisterVM model);
        public Task<Result> Login(LoginVM model);
        public Task Logout();
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private VueHwDbContext _db;

        private readonly IUserStore<AppUser> _userStore;
        private readonly IUserEmailStore<AppUser> _emailStore;
        // private readonly IEmailSender _emailSender;


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        private readonly IJwtService _jwtService;

        /// <summary>
        /// AuthService 註冊 登入 登出
        /// </summary>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        /// <param name="db"></param>
        /// <param name="emailSender"></param>
        /// <param name="userStore"></param>
        public AuthService(
                SignInManager<AppUser> signInManager,
                UserManager<AppUser> userManager,
                RoleManager<AppRole> roleManager,
                VueHwDbContext db,
                // IEmailSender emailSender,
                IUserStore<AppUser> userStore,
                IJwtService jwtService
        )
        {

            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
            _userStore = userStore;
            _emailStore = _GetEmailStore();
            // _emailSender = emailSender;
            _jwtService = jwtService;
            _roleManager = roleManager;
        }


        public async Task<Result> Register(RegisterVM model)
        {
            return await _RegisterUserAndRole(model, SysRole.User);
        }

        public async Task<Result> RegisterAdmin(RegisterVM model)
        {
            return await _RegisterUserAndRole(model, SysRole.Admin);
        }

        private async Task<Result> _RegisterUserAndRole(RegisterVM model, SysRole role)
        {
            var res = new Result()
            {
                Success = false
            };

            using (var dbContextTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // https://learn.microsoft.com/zh-tw/aspnet/core/security/authentication/add-user-data?view=aspnetcore-7.0&tabs=visual-studio
                    // Create a new user
                    var iuser = CreateUser();
                    iuser.NormalizedUserName = model.Name;
                    iuser.Email = model.Email;
                    iuser.UserName = model.Email;
                    iuser.CreateTime = DateTime.Now;
                    iuser.ChiName = model.Name;

                    // await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                    // await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                    // Create the user within the transaction
                    IdentityResult result = await _userManager.CreateAsync(
                        iuser,
                        model.Pwd
                    );

                    if (result.Succeeded)
                    {

                        // add user role: admin/user
                        var roleResult = await _userManager.AddToRoleAsync(iuser, role.ToString());

                        if (!roleResult.Succeeded)
                        {
                            throw new Exception("add role fail");
                        }

                        var user = await _userManager.FindByEmailAsync(model.Email);

                        if (user != null)
                        {
                            // mail
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                            // TODO: 直接一註冊就先已驗證，待補寄信
                            await _userManager.ConfirmEmailAsync(user, code);

                            // TODO: 信箱未實作
                            // await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                            //     $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                            // res.Msg = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";

                            // 信箱未驗證
                            if (_userManager.Options.SignIn.RequireConfirmedAccount)
                            {
                                res.Msg = "信箱未驗證";
                            }

                            res.Success = true;
                            // If user creation is successful, commit the transaction
                            dbContextTransaction.Commit();
                        }
                        else
                        {
                            // If there's an error, roll back the transaction
                            dbContextTransaction.Rollback();
                            res.Success = false;
                            res.Msg = result.Errors;
                        }
                    }
                    else
                    {
                        res.Success = false;
                        res.Msg = result.Errors;
                    }
                }
                catch (Exception)
                {
                    // Handle exceptions here, and optionally roll back the transaction
                    dbContextTransaction.Rollback();
                    res.Success = false;
                    res.Msg = "rollback";
                }

                return res;
            }
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Result> Login(LoginVM model)
        {
            var res = new Result()
            {
                Success = false
            };

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Pwd, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                res.Success = true;
                var user = await _userManager.FindByNameAsync(model.Email);
                var ur = _db.UserRoles.FirstOrDefault(x => x.UserId == user.Id);
                var r = _db.Roles.Find(ur.RoleId);
                var roleName = await _roleManager.GetRoleNameAsync(r);
                res.Msg = await _jwtService.GenerateTokenAsync(user);
            }
            // if (result.RequiresTwoFactor)
            // {
            //     return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            // }
            // if (result.IsLockedOut)
            // {
            //     _logger.LogWarning("User account locked out.");
            //     return RedirectToPage("./Lockout");
            // }
            else
            {
                res.Success = false;
                res.Msg = "Invalid login attempt.";
            }
            return res;
        }

        private IUserEmailStore<AppUser> _GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<AppUser>)_userStore;
        }

        private AppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
                    $"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }

    public class Result
    {
        public bool Success { get; set; }
        public object? Msg { get; set; }
    }
}