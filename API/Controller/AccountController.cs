using System.Security.Cryptography;
using System.Text;
using API.Controller;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;

// using API.Interfaces;
// using AutoMapper;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {

        //reference to the database connection - "DataContext - manages database operations"
        //"DataContext context" - injects the database connection when the controller is created
        //This setup ensures the controller can communicate with the database without manually creating a new connection each time
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }


         //method name - "Register
        [HttpPost("register")] // POST: api/account/register?username=dave&password=pwd
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            

            if (string.IsNullOrWhiteSpace(registerDto.Username))
                return BadRequest("Username is required.");

            if (await UserExists(registerDto.Username)) 
                return BadRequest("Username is taken");

            if (registerDto.Password.Length < 6)
                return BadRequest("Password must be at least 6 characters long.");

            using var hmac = new HMACSHA512(); //cryptographic hashing algorithm used for secure password storage.

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //creation of user object
            var user = new AppUser{
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), //Converts the password into bytes
                PasswordSalt = hmac.Key

            };

            _context.Users.Add(user); //Adds the new user to the database but does not save it yet
            //saves the new user to the database
            try{
                await _context.SaveChangesAsync();
            }
            catch(Exception ex){
                return StatusCode(500, "Internal server error: " + ex.Message);
            } 
           

           return new UserDTO
           {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
           };
            
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < ComputeHash.Length; i++)
            {
                if (ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
                
            }

            return new UserDTO
           {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
           };
            
        }

        //checks if the username already exists in the database, helps prevent duplicate usernames
        private async Task<bool> UserExists(string username){
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

       
    }
}