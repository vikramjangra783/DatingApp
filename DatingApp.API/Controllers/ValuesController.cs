using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext dbContext;

         private readonly IAuthRepository repository;
        private readonly IConfiguration configuration;

        public ValuesController(DataContext dbContext,IAuthRepository repo, IConfiguration config)
        {
            this.dbContext = dbContext;
            this.repository = repo;
            this.configuration = config;
        }
        // GET api/values
        [HttpGet]
        public async Task<IActionResult>  GetValues()
        {
            var values= await this.dbContext.Values.ToListAsync();
            return Ok(values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetValues(int id)
        {
            var values = await this.dbContext.Values.FirstOrDefaultAsync(x=> x.Id.Equals(id));
            return Ok(values);
        }

        // POST api/values
        // [HttpPost]
        // public void Post([FromBody] string value)
        // {
        // }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

         [HttpPost("register")]
         [AllowAnonymous]
        private async Task<IActionResult> PostRegister(UserForRegistrationDto user)
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

            var createdUser = await this.repository.Register(userToCreate, user.Password);

            return StatusCode(201);
        }
    }
}
