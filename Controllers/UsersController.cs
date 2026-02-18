    using BadgeCraft_Net.Data;
    using BadgeCraft_Net.DTOs;
    using BadgeCraft_Net.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;


    namespace BadgeCraft_Net.Controllers
    {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase

    {
        private readonly AppDbContext _context;

            public UsersController(AppDbContext context)
            {
                _context = context;
            }

            private int GetOrgId()
            {
                var orgClaim = User.Claims
                    .FirstOrDefault(c => c.Type == "organizationId");

                if (orgClaim == null)
                    throw new UnauthorizedAccessException("organizationId claim missing");

                return int.Parse(orgClaim.Value);
            }



            // GET ALL USERS (same organization)
            [HttpGet]
            public async Task<IActionResult> GetUsers()
            {
                var orgId = GetOrgId();

                var users = await _context.Users
                    .Where(u => u.OrganizationId == orgId)
                    .Select(u => new
                    {
                        u.Id,
                        u.Email,
                        u.Role
                    })
                    .ToListAsync();

                return Ok(users);
            }

            // CREATE USER
            [HttpPost]
            public async Task<IActionResult> Create(CreateUserDto dto)
            {
                var orgId = GetOrgId();

                var user = new User
                {
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Role = dto.Role,
                    OrganizationId = orgId
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    user.Id,
                    user.Email,
                    user.Role
                });
            }

            // UPDATE USER
            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, UpdateUserDto dto)
            {
                var orgId = GetOrgId();

                var user = await _context.Users
                    .FirstOrDefaultAsync(u =>
                        u.Id == id &&
                        u.OrganizationId == orgId);

                if (user == null)
                    return NotFound();

                user.Email = dto.Email;
                user.Role = dto.Role;

                await _context.SaveChangesAsync();

                return Ok();
            }

            // DELETE USER
            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var orgId = GetOrgId();

                var user = await _context.Users
                    .FirstOrDefaultAsync(u =>
                        u.Id == id &&
                        u.OrganizationId == orgId);

                if (user == null)
                    return NotFound();

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok();
            }

            [Authorize]
            [HttpGet("claims")]
            public IActionResult ClaimsDebug()
            {
                return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
            }


        }

    }

















