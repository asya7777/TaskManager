using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.DTOs;
using TaskManager.Entities;

using System.Security.Cryptography;//password hashing için
using System.Text;
using System.Reflection.Metadata.Ecma335;//stringleri byte dizisine çevirmek için

namespace TaskManager.Controllers
{

    [ApiController]
    [Route("api/[controller]")]


    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;//to read from my database
        public UserController(AppDbContext context)
        {
            _context = context;//receives my database connection and stores it in the private variable
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDTO dto)
        {
            using var hmac = new HMACSHA512();//hasher oluşturuyoruz

            byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.password));//turn the password into a byte array and hash it
            byte[] passwordSalt = hmac.Key;//the key used for hashing(salt)

            var newUser = new User
            {
                email = dto.email,
                firstName = dto.firstName,
                lastName = dto.lastName,
                passwordHash = passwordHash,
                passwordSalt = passwordSalt
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDTO dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(e => e.email == dto.email);

            if (user == null)
            {
                return Unauthorized("Invalid email.");
            }

            using var hmac = new HMACSHA512(user.passwordSalt);//user'ın salt'ını kullanarak hasher oluşturuyoruz
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.password));//karşılaştırma için gelen passwordü byte dizisine çevirip hashliyoruz

            for (int i = 0; i < computedHash.Length; i++)//byte by byte karşılaştırma
            {
                if (computedHash[i] != user.passwordHash[i])
                {
                    return Unauthorized("Invalid password.");
                }
            }

            return Ok(new
            {
                userId = user.usrId//returns the user ID if login is successful
            });
        }

        [HttpGet("get_all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new { u.usrId, u.firstName, u.lastName })
                .ToListAsync();

            return Ok(users);
        }
    }
}
