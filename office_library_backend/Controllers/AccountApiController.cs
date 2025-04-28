using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using office_library_backend.Models;
using Microsoft.Owin;
using Microsoft.AspNet;

namespace office_library_backend.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountApiController : ApiController
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;

        public AccountApiController()
        {
        }

        public AccountApiController(ApplicationUserManager userManager,
                                  ApplicationSignInManager signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }

        private ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await SignInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    var user = await UserManager.FindByEmailAsync(model.Email);
                    return Ok(new
                    {
                        userId = user.Id,
                        userName = user.UserName,
                        roles = await UserManager.GetRolesAsync(user.Id)
                    });

                case SignInStatus.LockedOut:
                    return Content(HttpStatusCode.Forbidden, "Account locked");

                case SignInStatus.Failure:
                default:
                    return Unauthorized();
            }
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            var result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors.ToString());

            // Назначение роли
            var roleResult = await UserManager.AddToRoleAsync(user.Id, model.Role);
            if (!roleResult.Succeeded)
                return InternalServerError();

            // Автоматический вход после регистрации
            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            return Created("", new
            {
                userId = user.Id,
                userName = user.UserName
            });
        }

        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            var authentication = Request.GetOwinContext().Authentication;
            authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                }
                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}