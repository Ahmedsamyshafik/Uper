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
        private readonly IHttpContextAccessor _contextAccessor;
        #endregion

        #region Ctor
        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
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
                Gender = model.Gender,
                Address=model.Address,
                IsSmoking= model.IsSmoking,
                CarType = model.CarType,
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
            //Save Image
            if(model.Image != null && model.Image.Length > 0)
            {
                var host = _contextAccessor.HttpContext.Request.Host;
                var schema = _contextAccessor.HttpContext.Request.Scheme;
               var response= await SavingImage(model.Image,schema,host);
                if (response.success)
                {
                    user.imageUrl = response.messageOrUrl;
                    await _userManager.UpdateAsync(user);
                }
                else
                {
                    return new RegisterErrorResponeDTO() { error = "Image", message = response.messageOrUrl };
                }
            }

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
                UserName = user.UserName,
                ImageUrl=user.imageUrl
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
                temp.ImageUrl=user.imageUrl;
                
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

        #region Private Function
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
        private class responsing
        {
            public bool success;
            public string messageOrUrl;
            public string name;

        }

        private async Task<responsing> SavingImage(IFormFile imageFile, string requestSchema, HostString hostString)
        {
            string _uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            try
            {
                

                // Ensure the uploads directory exists
                Directory.CreateDirectory(_uploadsDirectory);

                // Generate a unique file name
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

                // Combine the uploads directory with the unique file name
                var filePath = Path.Combine(_uploadsDirectory, uniqueFileName);

                // Save the uploaded file to the uploads directory
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                // Construct the URL to access the uploaded file
                //  var imageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{uniqueFileName}";
                var imageUrl = $"{requestSchema}://{hostString}/images/{uniqueFileName}";


                // Return the URL of the uploaded image
                return new responsing() { success = true, messageOrUrl = imageUrl, name = uniqueFileName };

            }
            catch (Exception ex)
            {
                return new responsing() { success = false, messageOrUrl = $"An error occurred: {ex.Message}" };


            }
        }


        #endregion
    }
}
