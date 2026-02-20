using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BadgeCraft_Net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CsvController : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var records = new List<Dictionary<string, string>>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var rows = csv.GetRecords<dynamic>().Take(5);

                foreach (var row in rows)
                {
                    var dict = new Dictionary<string, string>();

                    foreach (var prop in (IDictionary<string, object>)row)
                    {
                        dict[prop.Key] = prop.Value?.ToString();
                    }

                    records.Add(dict);
                }
            }

            return Ok(records);
        }
    }
}