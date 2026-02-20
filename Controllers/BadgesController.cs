using BadgeCraft_Net.Data;
using BadgeCraft_Net.DTOs;
using BadgeCraft_Net.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BadgeCraft_Net.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BadgesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BadgesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBadge([FromForm] BadgeCreateDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                string imagePath = null;

                if (dto.Logo != null)
                {
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Logo.FileName);
                    var fullPath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await dto.Logo.CopyToAsync(stream);
                    }

                    imagePath = "/images/" + fileName;
                }

                var badge = new Badge
                {
                    Title = dto.Title,
                    Subtitle = dto.Subtitle,
                    BgColor = dto.BgColor,
                    TextColor = dto.TextColor,
                    ImageUrl = imagePath,
                    OrganizationId = dto.OrganizationId,
                    CreatedBy = userId
                };

                _context.Badges.Add(badge);
                await _context.SaveChangesAsync();

                return Ok(badge);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}