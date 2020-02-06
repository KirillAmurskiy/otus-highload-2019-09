using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SocialNetwork.App;
using SocialNetwork.App.Dtos;


namespace SocialNetwork.AspNet.Controllers
{
    
    /// <summary>
    /// Auth service
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IRegistrationService registrationSvc;
        private readonly IAuthService authSvc;

        public AuthController(IRegistrationService registrationSvc, IAuthService authSvc)
        {
            this.registrationSvc = registrationSvc;
            this.authSvc = authSvc;
        }
        
        /// <summary>
        /// Register new user.
        /// </summary>
        /// <param name="data">Data of new user.</param>
        /// <returns>Access token</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(RegistrationUserResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegistrationUserResult>> RegisterUser(RegisterUserData data)
        {
            var result = await registrationSvc.RegisterUser(data);
            return Ok(result);
        }

        /// <summary>
        /// Login an user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Access token</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenDto>> Login(
            [BindRequired]
            string email, 
            [BindRequired]
            string password)
        {
            return await authSvc.IssueToken(email, password);
        }

        /// <summary>
        /// Refresh an access token.
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns>Access token</returns>
        [HttpPost("token/refresh")]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenDto>> RefreshToken(
            [BindRequired]
            string refreshToken)
        {
            return await authSvc.RefreshToken(refreshToken);
        }

        /// <summary>
        /// Reset either an refresh token or all refresh tokens by userId.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="refreshToken"></param>
        /// <returns>Nothing</returns>
        [HttpDelete("token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ResetToken(
            long? userId,
            string refreshToken)
        {
            if (userId.HasValue)
            {
                await authSvc.ResetAllTokens(userId.Value);    
            }
            else if (!string.IsNullOrEmpty(refreshToken))
            {
                await authSvc.ResetToken(refreshToken);
            }
            
            return Ok();
        }
    }
}