using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeaApp.API.Contracts;
using IdeaApp.API.Data;
using IdeaApp.API.Dtos;
using IdeaApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdeaApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }
       

        // POST api/values
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegister)
        {  
             userForRegister.UserName = userForRegister.UserName.ToLower();

             if(await _authService.UserExists(userForRegister.UserName))
                return BadRequest("Username already exists");

             var newUser = new User{

                 UserName = userForRegister.UserName
             } ;  

             await _authService.Register(newUser,userForRegister.Password);

             return StatusCode(201);
        }

      
    }
}