using AutoMapper;
using DatingAppApi.Data;
using DatingAppApi.Dtos;
using DatingAppApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingAppApi.Controllers
{
    //[ServiceFilter(typeof(LogUserActivity))]
    //[Authorize]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IDataRepository _Repo;
        private readonly IMapper _Mapper;

        public UsersController(IDataRepository repo, IMapper mapper)
        {
            _Repo = repo;
            _Mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _Repo.GetUsers();

            var userToReturn = _Mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(userToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _Repo.GetUser(id);

            var userToReturn = _Mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserForUpdateDto userForUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var claimUserId = 0;
            var currentUserId = 0;

            //if (User.FindFirst(ClaimTypes.NameIdentifier).Value != null)
            if ((User.FindFirst(ClaimTypes.NameIdentifier).Value).Length.ToString() != null)
            {
                claimUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                currentUserId = claimUserId;

                var userFromRepo = await _Repo.GetUser(id);

                if (userFromRepo == null)
                {
                    return NotFound($"Could not find user with ID of {id}");
                }

                if (currentUserId != userFromRepo.Id)
                {
                    return Unauthorized();
                }

                _Mapper.Map(userForUpdateDto, userFromRepo);

                if (await _Repo.SaveAll())
                {
                    return NoContent();
                }

                throw new Exception($"Updating user {id} failed on save!");
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}
