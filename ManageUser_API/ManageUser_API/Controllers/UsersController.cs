using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManageUser_API.Models;
using ManageUser_API.ViewModels;
using System.Text;

namespace ManageUser_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }
        //search user theo tên, theo thứ tự tuổi giảm dần và trạng thái isActive=true
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<UserVM>>> GetSearchNames([FromQuery(Name = "name")] string name, int pageSize, int pageIndex, string sortType, string sortKey, bool isActive)
        {

            List<User> searchUsers = new List<User>();
            if(name != null)
            {
                switch (sortKey)
                {
                    case "age":
                        if (sortType == "DESC")
                        {
                            if (isActive == true)
                            { 
                                searchUsers = await _context.Users.Where(u => u.Name.ToLower().Contains(name.ToLower()) && u.isActive == true).OrderByDescending(u => u.Age).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(); 
                            }

                            else
                                searchUsers = await _context.Users.Where(u => u.Name.ToLower().Contains(name.ToLower()) && u.isActive == false).OrderByDescending(u => u.Age).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

                        }
                        else
                        {
                            if (isActive == true)
                            {
                                searchUsers = await _context.Users.Where(u => u.Name.ToLower().Contains(name.ToLower()) && u.isActive == true).OrderBy(u => u.Age).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

                            }
                            else
                                searchUsers = await _context.Users.Where(u => u.Name.ToLower().Contains(name.ToLower()) && u.isActive == false).OrderBy(u => u.Age).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                        }
                        break;
                    case "name":
                        if (sortType == "DESC")
                        {
                            if(isActive == true)
                            {
                                searchUsers = await _context.Users.Where(u => u.Name.ToLower().Contains(name.ToLower()) && u.isActive == true).OrderByDescending(u => u.Name).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

                            }
                            else
                                searchUsers = await _context.Users.Where(u => u.Name.ToLower().Contains(name.ToLower()) && u.isActive == false).OrderByDescending(u => u.Name).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                        }
                        else
                        {
                            if (isActive == true)
                            {
                                searchUsers = await _context.Users.Where(u => u.Name.ToLower().Contains(name.ToLower()) && u.isActive == true).OrderBy(u => u.Name).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

                            }
                            else
                                searchUsers = await _context.Users.Where(u => u.Name.ToLower().Contains(name.ToLower()) && u.isActive == false).OrderBy(u => u.Name).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                        }
                        break;
                }

            }
            return searchUsers.Select(u => new UserVM() { User = u }).ToList();

        }
        //API sẽ truyền xuống dạng
        //GET /api/users? pageIndex = 1 & pageSize = 10 & searchKey = abc & sortType = DESC & sortKey = age & isActive = true
       
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        
    }
}
