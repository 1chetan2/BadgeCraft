using BadgeCraft_Net.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BadgeCraft_Net.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrganizationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/organizations
        [HttpGet]
        public async Task<IActionResult> GetOrganizations()
        {
            var orgId = User.Claims
                .FirstOrDefault(c => c.Type == "OrganizationId")?.Value;

            if (orgId == null)
                return Unauthorized();

            var organizations = await _context.Organizations
                .Where(o => o.Id == int.Parse(orgId))
                .ToListAsync();

            return Ok(organizations);
        }
    }
}