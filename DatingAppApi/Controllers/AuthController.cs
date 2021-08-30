using DatingAppApi.Data;
using DatingAppApi.Dtos;
using DatingAppApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using AutoMapper;

namespace DatingAppApi.Controllers
{

    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _Repo;
        private readonly IConfiguration _Config;
        private readonly IMapper _Mapper;

        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _Repo = repo;
            _Config = config;
            _Mapper = mapper;
        }

        // GET: AuthController
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
        {
            // validate request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _Repo.UserExists(userForRegisterDto.Username))
            {
                ModelState.AddModelError("Username", " Username is already taken!");
            }

            var userToCreate = _Mapper.Map<User>(userForRegisterDto);

            User createdUser = await _Repo.Register(userToCreate, userForRegisterDto.Password);

            var userToReturn = _Mapper.Map<UserForDetailedDto>(createdUser);

            return CreatedAtRoute("GetUser", new { controler = "User", id = createdUser.Id }, userToReturn);

        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var userFromRepo = await _Repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

        //    if (userFromRepo == null)
        //    {
        //        return Unauthorized();
        //    }

        //    // generate (JWT) JSON Web Token
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    //var key = Encoding.ASCII.GetBytes("super secret key");
        //    var key = Encoding.ASCII.GetBytes(_Config.GetSection("AppSettings:Token").Value);

        //    // the token key is set on appsettings.json
        //    // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Config.GetSection("AppSettings:Token").Value));

        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
        //            new Claim(ClaimTypes.Name, userFromRepo.Username)
        //        }),

        //        Expires = DateTime.Now.AddDays(1),  // Token time expires 24 hours/1 day from creation time
        //        SigningCredentials = new SigningCredentials(
        //            new SymmetricSecurityKey(key),
        //            SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    // create token
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    var tokenString = tokenHandler.WriteToken(token);
        //    var user = _Mapper.Map<UserForListDto>(userFromRepo);
        //    return Ok(new { tokenString, user }); // pass token back as a new Objects
        //}


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _Repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            // generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_Config.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.Username)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var user = _Mapper.Map<UserForListDto>(userFromRepo);

            return Ok(new { tokenString, user });

        }

    }

}


