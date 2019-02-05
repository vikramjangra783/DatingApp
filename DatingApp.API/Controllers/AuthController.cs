using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repository;
        public AuthController(IAuthRepository repo)
        {
            this.repository = repo;
        }

        [HttpPost("register")]
        private async Task<IActionResult> Register(UserForRegistrationDto user)
        {
            user.Username = user.Username.ToLower();
            if (await this.repository.UserExists(user.Username))
            {
                return BadRequest("Username already exists");
            }

            var userToCreate = new User()
            {
                UserName = user.Username
            };

            var createdUser= await this.repository.Register(userToCreate,user.Password);

            return StatusCode(201);
        }

    }
}