using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdeaApp.API.Contracts;
using IdeaApp.API.Data;
using IdeaApp.API.Dtos;
using IdeaApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IdeaApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;

        public AuthController(IAuthService authService, IConfiguration config)
        {
            this._config = config;
            this._authService = authService;
        }


        // POST api/values
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegister)
        {
            userForRegister.UserName = userForRegister.UserName.ToLower();

            if (await _authService.UserExists(userForRegister.UserName))
                return BadRequest("Username already exists");

            var newUser = new User
            {

                UserName = userForRegister.UserName
            };

            await _authService.Register(newUser, userForRegister.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLogin){

            var user = await _authService.Login(userForLogin.UserName.ToLower(),userForLogin.Password);
            if(user==null)
               return Unauthorized();

            var claims = new[]{
                  new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                  new Claim(ClaimTypes.Name , user.UserName)
            };

           
            var base64Encoding = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(base64Encoding));
            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }


    }
}