using System.Threading.Tasks;
using System.Collections.Generic;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    // Only Authorize users can send request to this API
    // You can communicate with the API using the URL api/user
    
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public UserController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailDto>(user);
            return Ok(userToReturn);
        }


    }
}