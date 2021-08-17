using AutoMapper;
using DatingAppApi.Data;
using DatingAppApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppApi.Controllers
{
    //[Authorize]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _Repo.GetUser(id);

            var userToReturn = _Mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }
    }
}
