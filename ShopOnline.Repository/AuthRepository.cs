using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShopOnline.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext dbContext;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthRepository(DataContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> IsUserExistAsync(string email)
        {
            if (await dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower()) != null)
                return true;
            else
                return false;
        }

        public async Task<ServiceResponse<string>> LoginAsync(string email, string password)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            ServiceResponse<string> response = new ServiceResponse<string>();

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Data = CreateToken(user);
            }
            else
            {
                response.Success = false;
                response.Message = "Wrong password.";
            }
            return response;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("TokenStrings:secret").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }

        public async Task<ServiceResponse<int>> RegisterAsync(User user, string password)
        {
            if (await IsUserExistAsync(user.Email))
            {
                return new ServiceResponse<int>
                {
                    Message = "User already exists.",
                    Success = false
                };
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return new ServiceResponse<int>
            {
                Data = user.Id,
                Success = true,
                Message = "Registered successfully."
            };
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash =
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public async Task<ServiceResponse<bool>> ChangePasswordAsync(int userId, string newPassword)
        {
            var response = new ServiceResponse<bool>();

            var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                await dbContext.SaveChangesAsync();
                response.Data = true;
                response.Message = "Password has been changed.";
            }
            else
            {
                response.Success = false;
                response.Message = "User not found.";
            }

            return response;
        }

        public int GetUserId() =>
            int.Parse(httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public string GetUserEmail() =>
          httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;

        public async Task<User> GetUserByEmailAsync(string email) =>
         await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

    }
}
