using FirstAPI.Data;
using FirstAPI.DTO;
using FirstAPI.Entities;
using FirstAPI.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace FirstAPI.Services
{
    public class AuthService(ApplicationDBContext context) : IAuthService
    {
        public async Task<Tuple<int, string>> LoginUser(UserDTO userdto)
        {
            try
            {
                if(userdto == null || string.IsNullOrEmpty(userdto.Email) || string.IsNullOrEmpty(userdto.Password))
                {
                    return new Tuple<int, string>(1, "Invalid user data");    // 1 = Invalid input
                }
                var existingUser = await context.AccountUsers.FirstOrDefaultAsync(u => u.Email == userdto.Email);

                if (existingUser == null)
                {
                    return new Tuple<int, string>(0, "User not found");     // 0 = Not Found
                }

                if (string.IsNullOrEmpty(existingUser.Password))
                {
                    return new Tuple<int, string>(4, "Incorrect password, Please try again");  // 4 = Wrong password
                }

                var passwordHasher = new PasswordHasher<string>();
                var verificationResult = passwordHasher.VerifyHashedPassword(userdto.Email, existingUser.Password, userdto.Password);

                if (verificationResult == PasswordVerificationResult.Success)
                {
                    return new Tuple<int, string>(2, "Login Successful");
                }

                else if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
                {
                   existingUser.Password = PasswordHashing(userdto);

                    context.AccountUsers.Update(existingUser);
                    await context.SaveChangesAsync();
                    return new Tuple<int, string>(2, "Login Successful, password hash updated");  // 2 = Success
                }
                else if (verificationResult == PasswordVerificationResult.Failed)
                {
                    return new Tuple<int, string>(4, "Incorrect password, Please try again");  // 4 = Wrong password
                }
                   
                return new Tuple<int, string>(1, "Password verification failed");  // 1 = Invalid input
                
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
                    return new Tuple<int, string>(1, "Invalid user data");   // 1 = Invalid input
                }
                var existingUser = await context.AccountUsers.AnyAsync(u => u.Email == userdto.Email);
                if (existingUser)
                {
                    return new Tuple<int, string>(3, "This user already exist");   // 3 = Conflict
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
    }
}
