using Microsoft.AspNetCore.Mvc;

using Web.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleController : ControllerBase
    {
        private readonly GoogleAIService _googleAIService;
        public GoogleController(GoogleAIService googleAIService) { 
            _googleAIService = googleAIService;
        
        }
        // GET: api/<GoogleController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<GoogleController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<GoogleController>
        [HttpPost]
        public async Task<ActionResult<string>> Post(IFormFile file)
        {
            try
            {
                string result = await _googleAIService.IdentifyObject(await _googleAIService.ConvertFileStreamToImage(file));
                return Ok(result);
            } catch (Exception ex)
            {
                return BadRequest(ex);
            }
    
        }

        // PUT api/<GoogleController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GoogleController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
