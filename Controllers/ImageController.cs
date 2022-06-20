using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NetVips;
using System.Collections.Generic;

namespace BoxOffice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        [HttpGet("new-image")]
        public async Task<IActionResult> GetNewImage()
        {
            var image = Image.NewFromFile(@"pp.jpg", access: Enums.Access.Sequential);
            //image *= new[] { 1, 1, 1 };
            var mask = Image.NewFromArray(new[,] {
                {-1, -1, -1},
                {-1, 15, -1},
                {-1, -1, -1}
            }, scale: 14);

            image = image.Conv(mask, precision: Enums.Precision.Integer);

            return File(image.JpegsaveBuffer(),
                        "image/jpeg",
                        $"test.jpg"
                        );
        }

        [HttpGet("compress")]
        public async Task<IActionResult> compress()
        {
            var image = Image.NewFromFile(@"pp.jpg");
            image.DzsaveBuffer();
            return File(image.JpegsaveBuffer(q: 20, optimizeCoding: true, interlace: true, trellisQuant: true, overshootDeringing: true, optimizeScans: true),
                        "image/jpeg",
                        $"test.jpg"
                        );
        }

        [HttpGet("metadata")]
        public async Task<IActionResult> GetMetadata()
        {
            var image = Image.NewFromFile(@"pp.jpg", access: Enums.Access.Sequential);

            var metadata = new Dictionary<string, string>();

            foreach (var item in image.GetFields())
            {
                try
                {
                    metadata.TryAdd(item, image.Get(item).ToString());
                }
                catch (System.Exception)
                {
                    metadata.TryAdd(item, "");
                }               
            }

            return Ok(metadata);
        }
    }
}
