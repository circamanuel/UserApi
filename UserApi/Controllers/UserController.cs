using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.Models;
using UserApi.DTOs;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        public  UserController(AppDbContext context) 
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser(CreateUserDTO CreateUserDTO)
        {
            var user = new User
            {
                FirstName = CreateUserDTO.FirstName,
                LastName = CreateUserDTO.LastName,
                Email = CreateUserDTO.Email,
                IsActive = CreateUserDTO.IsActive,
                Password = CreateUserDTO.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            // note machen wie das genau funktioniert!!!!
            // umwandeln in dto damit wir den get request machen konnen
            return CreatedAtAction(
                nameof(GetUser), 
                new { Id = user.Id }, 
                UserToDTO(user));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await _context.Users.Select(x => UserToDTO(x)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id); 

            if (user == null)
            {
                return NotFound();
            }

            return UserToDTO(user);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> UpdateUser(int id, User userDTO)
        {
            if (id != userDTO.Id)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.Email = userDTO.Email;
            user.IsActive = userDTO.IsActive;

            // Entry holt den tracking eintrag fur users
            // Stat = EntryState.Modified => User wurde geendert bitte updaten. EF weiss nicht das das ein geenderter user ist muss also manuell gemacht werden
            //_context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UserExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private static UserDTO UserToDTO(User user) => new UserDTO
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            IsActive = user.IsActive
        };
    }
}
