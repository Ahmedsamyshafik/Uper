using Driver.DTOs.UserDTos.Admination;
using Driver.DTOs.UserDTos.Login;
using Driver.DTOs.UserDTos.Register;
using Driver.Helpers;
using Driver.Models;
using Driver.Service.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Driver.Service.Services
{
    public class AuthService : IAuthService
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;

        }


        #endregion

        #region Handle Functions

        #region Login/Register
        public async Task<RegisterErrorResponeDTO> RegisterAsync(RegisterDTO model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null || await _userManager.FindByNameAsync(model.UserName) != null)
            { // User with same email

                return new RegisterErrorResponeDTO() { message = "Existing", error = "Email Or Name is Already Registered" };
            }

            //maping from RegisterDTO to ApplicationUser
            var user = new ApplicationUser() // Role - Password 
            {
                Email = model.Email,
                UserName = model.UserName,
                Region = model.Region,
                Gender = model.Gender
            };
            //Create User  
            var result = await _userManager.CreateAsync(user, model.Password);

            // Complete Comment and mapping
            if (!result.Succeeded)
            {
                var Es = string.Empty;
                foreach (var error in result.Errors)
                {
                    Es += $"{error.Description} ,";
                }
                return new RegisterErrorResponeDTO() { message = "Password", error = Es };

            }
            await _userManager.UpdateAsync(user);//To Make Sure thier is id! "كانت ساعات بتعمل ايرور"

            //Seed Role "User-Driver"
            var y = await _userManager.AddToRoleAsync(user, model.UserType);
            await _userManager.UpdateAsync(user);
            return new RegisterErrorResponeDTO() { message = "Success" };
        }
        public async Task<LoginResponseDTO> Login(LoginDTO model)
        {
            LoginResponseDTO returnedUser = new();

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                returnedUser.Message = "Name Or Password is invalid..!";
                returnedUser.Email = model.Email;
                return returnedUser;
            }
            if (!user.IsActive)
            {
                returnedUser.Message = "Not Active !";
                returnedUser.Email = model.Email;
                return returnedUser;
            }
            //JWT
            var JwtSecuirtyToken = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            //map from applicationUser => LoginResponseDTO
            var returned = new LoginResponseDTO()
            {
                CarType = user.CarType,
                Address = user.Address,
                Email = user.Email,
                Gender = user.Gender,
                IsAuthenticated = true,
                Region = user.Region,
                userId = user.Id,
                UserName = user.UserName
            };
            //roleing
            if (roles.Contains(Constants.AdminRole))
            {
                returned.Role = Constants.AdminRole;
            }
            else if (roles.Contains(Constants.DriverRole))
            {
                returned.Role = Constants.DriverRole;
            }
            else
            {
                returned.Role = Constants.UserRole;
            }
            returned.Expier = JwtSecuirtyToken.ValidTo;
            returned.Token = new JwtSecurityTokenHandler().WriteToken(JwtSecuirtyToken);
            return returned;
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("userId", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var SigingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddDays(7), // Expire date..
                signingCredentials: SigingCredentials
                );
            var myToken = new
            {
                _token = new JwtSecurityTokenHandler().WriteToken(token), // To make Token Json
                expiration = token.ValidTo
            };
            return token;
        }
        #endregion

        #region Accept Users

        public async Task<List<ListUsers>> ListOfUsers()
        {
            var lst = _userManager.Users.ToList().Where(x => x.IsActive == false);
            var result = new List<ListUsers>();
            foreach (var user in lst)
            {
                var temp = new ListUsers();
                temp.gender = user.Gender;
                temp.address = user.Address;
                temp.region = user.Region;
                temp.email = user.Email;
                temp.name = user.UserName;
                var roles = await _userManager.GetRolesAsync(user);
                temp.Role = roles.FirstOrDefault();
                temp.id = user.Id;
                result.Add(temp);
            }
            return result;
        }

        public async Task<string> UserActivation(UserActivation activation)
        {
            var user = await _userManager.FindByIdAsync(activation.Id);
            if (user == null) return "Not Exist";
            if (!activation.Active)
            {
                await _userManager.DeleteAsync(user);
            }
            user.IsActive = true;
            await _userManager.UpdateAsync(user);
            return "Success";
        }
            #endregion

        public async Task<ApplicationUser> GetUserById(string userid)
        {
            return await _userManager.FindByIdAsync(userid);
        }

            #endregion
        }
    }
