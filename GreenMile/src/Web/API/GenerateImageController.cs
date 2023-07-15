using System.Diagnostics.Metrics;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Web.Services;

namespace Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateImageController : ControllerBase
    {
        private readonly OpenAIApiService _openAIApiService;
        public  GenerateImageController(OpenAIApiService openAIApiService)
        {
            _openAIApiService = openAIApiService;
        }
        // GET: api/generateimage/50000
        [HttpPost]
        public async Task<ActionResult<string>> GenerateImage([FromBody]object prompt)
        {
            try
            {
                string image = await _openAIApiService.GenerateImage(prompt.ToString());
                return Ok(image);
            }
            catch (Exception)
            {
                return BadRequest("https://upload.wikimedia.org/wikipedia/commons/thumb/2/2c/Default_pfp.svg/2048px-Default_pfp.svg.png");
            }
        }
    }
}
