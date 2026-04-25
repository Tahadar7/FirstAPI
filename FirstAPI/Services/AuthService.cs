using FirstAPI.Data;
using FirstAPI.DTO;
using FirstAPI.Entities;
using FirstAPI.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace FirstAPI.Services
{
    public class AuthService(ApplicationDBContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<Tuple<int, TokenDTO>> LoginUser(UserDTO userdto)
        {
            try
            {
                var tokenDTO = new TokenDTO();

                if (userdto == null || string.IsNullOrEmpty(userdto.Email) || string.IsNullOrEmpty(userdto.Password))
                {
                    tokenDTO.Message = "User data is empty";
                    return new Tuple<int, TokenDTO>(1, tokenDTO);    // 1 = Invalid input
                }
                var existingUser = await context.AccountUsers.FirstOrDefaultAsync(u => u.Email == userdto.Email);

                if (existingUser == null)
                {
                    tokenDTO.Message = "User not found";
                    return new Tuple<int, TokenDTO>(0, tokenDTO);        // 0 = Not Found
                }

                if (string.IsNullOrEmpty(existingUser.Password))
                {
                    tokenDTO.Message = "User has no password set";
                    return new Tuple<int, TokenDTO>(4, tokenDTO);       // 4 = Wrong password
                }

                var passwordHasher = new PasswordHasher<string>();
                var verificationResult = passwordHasher.VerifyHashedPassword(userdto.Email, existingUser.Password, userdto.Password);

                if (verificationResult == PasswordVerificationResult.Success)
                {
                    var token = GenerateJWTtoken(existingUser.Id, existingUser.Name, existingUser.Email);

                    tokenDTO.Token = token;
                    tokenDTO.Message = "Login Successful";

                    return new Tuple<int, TokenDTO>(2, tokenDTO);
                }

                else if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    var token = GenerateJWTtoken(existingUser.Id, existingUser.Name, existingUser.Email);

                    existingUser.Password = PasswordHashing(userdto);

                    context.AccountUsers.Update(existingUser);
                    await context.SaveChangesAsync();

                    tokenDTO.Token = token;
                    tokenDTO.Message = "Login Successful, password hash updated";

                    return new Tuple<int, TokenDTO>(2, tokenDTO);  // 2 = Success
                }
                else if (verificationResult == PasswordVerificationResult.Failed)
                {
                    tokenDTO.Message = "Wrong password";
                    return new Tuple<int, TokenDTO>(4, tokenDTO);  // 4 = Wrong password
                }

                tokenDTO.Message = "Password verification failed";

                return new Tuple<int, TokenDTO>(1, tokenDTO);     // 1 = Invalid input
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Tuple<int, string>> RegisterUser(UserDTO userdto)
        {
            try
            {
                if (userdto == null || string.IsNullOrEmpty(userdto.Email) || string.IsNullOrEmpty(userdto.Password) || string.IsNullOrEmpty(userdto.Name))
                {
                    return new Tuple<int, string>(1, "User data is empty");          // 1 = Invalid input
                }
                var existingUser = await context.AccountUsers.AnyAsync(u => u.Email == userdto.Email);
                if (existingUser)
                {
                    return new Tuple<int, string>(3, "This user already exist");        // 3 = Conflict
                }

                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Name = userdto.Name,
                    Password = PasswordHashing(userdto),
                    Email = userdto.Email

                };

                context.AccountUsers.Add(newUser);
                await context.SaveChangesAsync();

                return new Tuple<int, string>(2, "User registered Successfully");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string PasswordHashing(UserDTO userdto)
        { 
            var passwordHasher = new PasswordHasher<string>();
            return passwordHasher.HashPassword(userdto.Email ?? string.Empty, userdto.Password ?? string.Empty);
        }

        private string GenerateJWTtoken(Guid userid, string name, string email)
        {
           var claims = new[]
           {
               new Claim(ClaimTypes.Name, name),
               new Claim(ClaimTypes.Email, email),
               new Claim(ClaimTypes.NameIdentifier, userid.ToString())
           };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

            var jwtHandler = new JwtSecurityTokenHandler();

            var encryptionCredentials = new EncryptingCredentials(key, 
                                SecurityAlgorithms.Aes256KW, SecurityAlgorithms.Aes256CbcHmacSha512);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = configuration["JWT:Issuer"],
                Audience = configuration["JWT:Audience"],
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                EncryptingCredentials = encryptionCredentials
            };
            
            var token = jwtHandler.CreateToken(tokenDescriptor);
            return jwtHandler.WriteToken(token);
        }
    }
}
