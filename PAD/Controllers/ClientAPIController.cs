using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PAD.Data;
using Syroot.Windows.IO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PAD.Controllers
{
    [Route("client/api/v1/")]
    [ApiController]
    public class ClientAPIController : Controller
    {
        private readonly IDbService _db;
        private readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _user;
        private readonly ICryptographyService _crypto;
        private readonly JWT_Helper _jwt;

        public ClientAPIController(IDbService dbService, IConfiguration configuration, UserManager<IdentityUser> userManager, ICryptographyService cryptographyService)
        {
            _db = dbService;
            _config = configuration;
            _user = userManager;
            _crypto = cryptographyService;
            _jwt = new JWT_Helper(_config, _user);
        }

        /// <summary>
        /// Authenticates a user.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns>IActionResult</returns>
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
        /// Returns the url of a random image if passed a valid token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>IActionResult</returns>
        [Route("image")]
        [HttpGet("details")]
        public async Task<IActionResult> GetRandomImage(string token)
        {
            if (string.IsNullOrEmpty(token)) return BadRequest("token cannot be null or empty");
            if (!_jwt.ValidateToken(token)) return BadRequest("token could not be validated");

            var image = await _db.GetAllImageListAsync();
            var rand = new Random(Environment.TickCount);
            var randResult = rand.Next(image.Count);

            return Ok(image[randResult].Url);

        }

        /// <summary>
        /// Downloads an image if passed a valid token and image id
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns>IActionResult</returns>
        [Route("image/save")]
        [HttpGet("details")]
        public async Task<IActionResult> SaveImage(string token, string id)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(id)) return BadRequest("token and id cannot be null or empty");
            if (!_jwt.ValidateToken(token)) return BadRequest("token could not be validated");
            if (!Guid.TryParse(id, out var imageId)) return BadRequest("could not parse valid image id");

            var image = await _db.GetImageAsync(imageId);

            if (image == null) return NotFound("invalid id, no such image exists");

            try
            {
                var bytes = await new WebClient().DownloadDataTaskAsync(image.Url);
                var wbc = await new WebpConverter().WebpConvert(bytes);
                var downloadsPath = new KnownFolder(KnownFolderType.Downloads).Path;
                var fullUrl = image.Url.Split("/");
                var name = fullUrl[fullUrl.Length - 1].Split(".");
                var location = await AzureStorage.UploadFile(name[0], wbc, "temp");
                new WebClient().DownloadFile(location, $"{downloadsPath}\\{name[0]}.webp");
                return Ok("image saved");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        /// <summary>
        /// Returns information on a specified image and its project if passed a valid token and project id
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("image/info")]
        [HttpGet("details")]
        public async Task<IActionResult> GetImageInfo(string token, string id)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(id)) return BadRequest("token and id cannot be null or empty");
            if (!_jwt.ValidateToken(token)) return BadRequest("token could not be validated");
            if (!Guid.TryParse(id, out var imageId)) return BadRequest("could not parse valid image id");

            var image = await _db.GetImageAsync(imageId);
            if (image == null) return NotFound("invalid id, no such image exists");
            var project = await _db.GetProjectAsync(image.ProjectId);
            var returnObject = new
            {
                DisplayTitle = project.DisplayTitle,
                Description = project.Description,
                GridSize = project.GridSize,
                OwnerId = project.AccountId,
                Url = image.Url,
                Published = image.CreateDate,
                LastUpdated = image.UpdateDate
            };

            return Ok(returnObject);

        }

        /// <summary>
        /// Returns the urls of all images if passed a valid token and no (or an empty) id,
        /// or returns the urls of all images belonging to a specific user is passed a valid id
        /// </summary>
        /// <param name="token"></param>
        /// <returns>IActionResult<string></returns>
        [Route("images")]
        [HttpGet("details")]
        public async Task<IActionResult> GetImages(string token, string id)
        {
            if (string.IsNullOrEmpty(token)) return BadRequest("token cannot be null or empty");
            if (!_jwt.ValidateToken(token)) return BadRequest("token could not be validated");

            if (string.IsNullOrEmpty(id))
            {
                var images = await _db.GetAllImageListAsync();
                List<string> urls = new List<string>();
                foreach (var image in images) { urls.Add(image.Url); }

                return Ok(urls);
            }

            if (!int.TryParse(id, out var userId)) return BadRequest("could not parse valid user id");

            var usersImages = await _db.GetImageListAsync(userId);
            if (usersImages.Count == 0) return NotFound("invalid user id, user does not exist or has no published images");

            List<string> usersUrls = new List<string>();
            foreach (var image in usersImages) { usersUrls.Add(image.Url); }

            return Ok(usersUrls);

        }

    }
}
