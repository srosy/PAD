using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PAD.Data;
using System;
using Syroot.Windows.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PAD.Controllers
{
    [Route("admin/api/v1/")]
    [ApiController]
    public class AdminAPIController : ControllerBase
    {
        private readonly IDbService _db;
        private readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _user;
        private readonly ICryptographyService _crypto;
        private readonly JWT_Helper _jwt;
        private readonly ILogger _logger;

        public AdminAPIController(IDbService db, IConfiguration config, UserManager<IdentityUser> user, ICryptographyService cryptographyService, ILogger logger)
        {
            _db = db;
            _config = config;
            _user = user;
            _crypto = cryptographyService;
            _jwt = new JWT_Helper(_config, _user);
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>IActionResult(</returns>
        [Route("authenticate")]
        [HttpPost("details")]
        public async Task<IActionResult> GetToken(string credentials)
        {
            if (string.IsNullOrEmpty(credentials)) return BadRequest("Credentials cannot be null or empty.");

            try
            {
                var creds = _crypto.Decrypt(credentials);
                var user = await _user.FindByEmailAsync(creds.email);
                var validUser = (user.PasswordHash == creds.hashedpass);
                if (validUser) return Ok(_jwt.Authenticate(user).GetAwaiter().GetResult());
                return Unauthorized("Invalid user");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return BadRequest("Invalid Credentials. Proceed to your PAD profile page to generate credentials.");
        }

        /// <summary>
        /// Adds the administrator role to the user associated with the provided email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("admin")]
        [HttpPost("details")]
        public async Task<IActionResult> MakeAdministrator(string email)
        {
            if (string.IsNullOrEmpty(email)) return BadRequest("email cannot be null or empty");

            try
            {
                var account = await _db.GetAccountByEmailAsync(email);
                var user = await _user.FindByIdAsync(account.UserId);
                await _user.AddToRoleAsync(user, "Administrator");
                return Ok(account.FirstName + " " + account.LastName + " has gained administrative privileges.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Bans a user associated with the provided email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("ban")]
        [HttpPost("details")]
        public async Task<IActionResult> BanAccount(string email)
        {
            if (string.IsNullOrEmpty(email)) return BadRequest("email cannot be null or empty");

            try
            {
                var account = await _db.GetAccountByEmailAsync(email);
                var user = await _user.FindByIdAsync(account.UserId);
                await _user.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                return Ok(account.FirstName + " " + account.LastName + " has been banned.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Unbans a user associated with the provided email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("unban")]
        [HttpPost("details")]
        public async Task<IActionResult> UnbanAccount(string email)
        {
            if (string.IsNullOrEmpty(email)) return BadRequest("email cannot be null or empty");

            try
            {
                var account = await _db.GetAccountByEmailAsync(email);
                var user = await _user.FindByIdAsync(account.UserId);
                await _user.SetLockoutEndDateAsync(user, DateTimeOffset.MinValue);
                return Ok(account.FirstName + " " + account.LastName + " has been unbanned.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Removes any inappropriate image associated to the provided Guid.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("strike")]
        [HttpPost("details")]
        // Might upgrade this to the image Url with an associated DB service query.
        public async Task<IActionResult> StrikeContent(Guid id)
        {
            if (id.Equals(Guid.Empty)) return BadRequest("id cannot be empty");

            try
            {
                var image = await _db.GetImageAsync(id);
                await _db.RemoveImageAsync(image.ImageId);
                return Ok(image.Url + " has been removed from the website");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Runs script to remove any deleted records older than 2 weeks.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("clean")]
        [HttpGet("details")]
        public async Task<IActionResult> CleanDatabase()
        {
            return Ok(await _db.CleanDB());
        }

        /// <summary>
        /// Adds the image located at the provided url to the db and blob storage
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("upload")]
        [HttpPost("details")]
        public async Task<IActionResult> UploadImage(string url, string name)
        {
            if (string.IsNullOrEmpty(url)) return BadRequest("url cannot be null or empty");
            if (string.IsNullOrEmpty(name)) return BadRequest("name cannot be null or empty");

            try
            {
                var image = new PAD.Data.Models.Image()
                {
                    CreateDate = DateTime.UtcNow,
                    AccountId = 95,
                    ProjectId = 55,
                    ImageId = Guid.NewGuid()
                };

                image.Url = await AzureStorage.PublishImage($"{name}-{0}.png", url);
                var published = await _db.AddImageAsync(image);

                if (published)
                {
                    return Ok(image.Url);
                }
                else return BadRequest("Image could not be published");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Downloads image in webp format
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("download")]
        [HttpPost("details")]
        public async Task<IActionResult> DownloadImage(string url, string name)
        {
            if (string.IsNullOrEmpty(url)) return BadRequest("url cannot be null or empty");
            if (string.IsNullOrEmpty(name)) return BadRequest("name cannot be null or empty");

            try
            {
                var bytes = await new System.Net.WebClient().DownloadDataTaskAsync(url);
                var wbc = await new WebpConverter().WebpConvert(bytes);
                var downloadsPath = new KnownFolder(KnownFolderType.Downloads).Path;
                var location = await AzureStorage.UploadFile(name, wbc, "temp");
                new System.Net.WebClient().DownloadFile(location, $"{downloadsPath}\\{name}.webp");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Tests the global exception handler and logger.
        /// </summary>
        /// <returns></returns>
        [Route("exception")]
        [HttpGet]
        public async Task<IActionResult> ExceptionTest()
        {
            _logger.LogInformation("Starting log exception test...");
            throw new Exception("Exception in admin api exception test.");
        }
    }
}
