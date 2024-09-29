using Manage_Personal_Expenses.Data;
using Manage_Personal_Expenses.Dto;
using Manage_Personal_Expenses.Mapper;
using Manage_Personal_Expenses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Manage_Personal_Expenses.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly RegisterMapper _reisterMapper;
        public AuthentificationController(AppDbContext appDbContext, IConfiguration configuration,RegisterMapper registerMapper)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
            _reisterMapper=  registerMapper;
        }

        private async Task<string> GenerateJwtToken(string email,int id)
        {
            var userFound= await _appDbContext.UserModels.FirstOrDefaultAsync(x => x.Email == email);
            if(userFound is null) { return null; }

            var claim = new[]
            {
                new Claim("UserId",id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));

            var credentials= new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token= new JwtSecurityToken(
                issuer: _configuration["JwtConfig:Issuer"],
                audience: _configuration["JwtConfig:Audience"],
                claims: claim,
                expires:DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto registerRequest)
        {
            if(await _appDbContext.UserModels.AnyAsync(x=> x.Email == registerRequest.Email)) { return Conflict("Email-ul exista deja"); }


            registerRequest.Password=BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
           
          var user=_reisterMapper.MapToUser(registerRequest);

            await _appDbContext.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("login")]

        public async Task<IActionResult>Login([FromBody] LoginDto loginRequest)
        {
            var loginUser=  await _appDbContext.UserModels.FirstOrDefaultAsync(x=> x.Email== loginRequest.Email);
            if(loginUser == null) { return  Unauthorized("Email sau Parola gresita"); }

            bool passwordValid=BCrypt.Net.BCrypt.Verify(loginRequest.Password,loginUser.Password);
             if(!passwordValid) { return Unauthorized("Email sau Parola gresita"); }

            var token= await GenerateJwtToken(loginUser.Email,loginUser.Id);

            if(token==null) { return Unauthorized("Erroare la generarea tokenului"); }

            return Ok(new {token});

          }

        


    }
}
