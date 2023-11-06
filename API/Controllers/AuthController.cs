using API.ViewModels;
using API.Services;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.RecaptchaEnterprise.V1;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private IConfiguration _configuration;

        private readonly RecaptchaEnterpriseServiceClient _recaptchaClient;
        private string _projectID;
        private string _recaptchaSiteKey;

        private readonly IAuthService _service;

        public AuthController(
                RecaptchaEnterpriseServiceClient re,
                ILogger<AuthController> logger,
                IConfiguration configuration,
                IAuthService service
        )
        {
            _recaptchaClient = re;
            _configuration = configuration;
            _projectID = _configuration.GetValue<string>("projectID") ?? "";
            _recaptchaSiteKey = _configuration.GetValue<string>("recaptchaSiteKey") ?? "";
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.RecaptchaToken != null && await _CreateAssessmentAsync(model.RecaptchaToken))
            {
                var res = await this._service.Register(model);
                if (res.Success)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(res.Msg);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 註冊 Admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // [HttpPost("RegisterAdmin")]
        private async Task<IActionResult> RegisterAdmin(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.RecaptchaToken != null && await _CreateAssessmentAsync(model.RecaptchaToken))
            {
                var res = await this._service.RegisterAdmin(model);
                if (res.Success)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(res.Msg);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.RecaptchaToken != null && await _CreateAssessmentAsync(model.RecaptchaToken, "login"))
            {
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                var res = await this._service.Login(model);
                if (res.Success)
                {
                    _logger.LogInformation($"user {model.Email} is login, Hurray ^_^");
                    return Ok(new { jwt = res.Msg });
                }
                else
                {
                    return BadRequest(res.Msg as string);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await this._service.Logout();
            _logger.LogInformation("User logged out.");
            return NoContent();
        }

        // 來自保哥 https://blog.miniasp.com/post/2023/01/15/How-to-Integrate-reCAPTCHA-Enterprise-with-ASPNET-Core
        // Create an assessment to analyze the risk of an UI action.
        // projectID: GCloud Project ID.
        // recaptchaSiteKey: Site key obtained by registering a domain/app to use recaptcha.
        // token: The token obtained from the client on passing the recaptchaSiteKey.
        // recaptchaAction: Action name corresponding to the token.
        private async Task<bool> _CreateAssessmentAsync(string token, string recaptchaAction = "register")
        {
            string projectID = "vue-hw-1694479802001";
            string recaptchaSiteKey = "6LcdrxooAAAAABpR6EJ4pQnp8r4J6E-mm0weKKLU";

            if (String.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("The `token` argument must not empty.");
            }

            ProjectName projectName = new ProjectName(projectID);

            // Build the assessment request.
            CreateAssessmentRequest createAssessmentRequest = new CreateAssessmentRequest()
            {
                Assessment = new Assessment()
                {
                    // Set the properties of the event to be tracked.
                    Event = new Google.Cloud.RecaptchaEnterprise.V1.Event()
                    {
                        SiteKey = recaptchaSiteKey,
                        Token = token,
                        ExpectedAction = recaptchaAction
                    },
                },
                ParentAsProjectName = projectName
            };

            Assessment response = await _recaptchaClient.CreateAssessmentAsync(createAssessmentRequest);

            // Check if the token is valid.
            if (response.TokenProperties.Valid == false)
            {
                _logger.LogInformation("The CreateAssessment call failed because the token was: " +
                    response.TokenProperties.InvalidReason.ToString());

                return false;
            }

            // Check if the expected action was executed.
            if (response.TokenProperties.Action != recaptchaAction)
            {
                _logger.LogInformation("The action attribute in reCAPTCHA tag is: " +
                    response.TokenProperties.Action.ToString());
                _logger.LogInformation("The action attribute in the reCAPTCHA tag does not " +
                    "match the action you are expecting to score");
                return false;
            }

            // Get the risk score and the reason(s).
            // For more information on interpreting the assessment,
            // see: https://cloud.google.com/recaptcha-enterprise/docs/interpret-assessment

            var score = response.RiskAnalysis.Score;

            _logger.LogInformation("The reCAPTCHA score is: " + score);

            foreach (RiskAnalysis.Types.ClassificationReason reason in response.RiskAnalysis.Reasons)
            {
                _logger.LogInformation(reason.ToString());
            }

            // https://developers.google.com/recaptcha/docs/analytics
            // This chart shows the average score on your site, which is designed to help you spot trends.
            // Scores range from 0.0 to 1.0, with 0.0 indicating abusive traffic and 1.0 indicating good traffic.
            // Sign up for reCAPTCHA v3 to gain more insights about your traffic.
            if (score > 0.5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}