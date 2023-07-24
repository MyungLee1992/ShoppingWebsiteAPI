using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShoppingWebsiteAPI.Data;
using ShoppingWebsiteAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShoppingWebsiteAPI.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly DataContext _context;

        public AuthController(IConfiguration configuration, IUserService userService, DataContext dataContext) {
            _configuration = configuration;
            _userService = userService;
            _context = dataContext;
        }

        [HttpGet, Authorize]
        public ActionResult<string> GetMe() {
            var userName = _userService.GetMyName();

            return Ok(userName);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request) {
            if (_context.Users.Any(u => u.UserName == request.UserName)) {
                return BadRequest("User already exsits.");
            };

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);



            var cart = new Cart();
            User user = new User {
                UserName = request.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Cart = cart,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request) {
            var user = _context.Users.Where(u => u.UserName == request.UserName).FirstOrDefault();
            if (user == null || user.UserName != request.UserName) {
                return BadRequest("User not found.");
            }

            if (! VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt)) {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);

            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken() {
            var refreshToken = Request.Cookies["refreshToken"];
            if (! user.RefreshToken.Equals(refreshToken)) {
                return Unauthorized("Invalid Refresh Token.");
            } else if (user.TokenExpires < DateTime.Now) {
                return Unauthorized("Token expired.");
            }

            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }

        private RefreshToken GenerateRefreshToken() {
            var refreshToken = new RefreshToken {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.Now,
                Expires = DateTime.Now.AddDays(7),
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken) {
            var cookieOptions = new CookieOptions {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        private string CreateToken(User user) {
            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration.GetSection("AppSettings:Token").Value
                ));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash,  out byte[] passwordSalt) {
            using (var hmac = new HMACSHA512()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) {
            using (var hmac = new HMACSHA512(passwordSalt)) {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
