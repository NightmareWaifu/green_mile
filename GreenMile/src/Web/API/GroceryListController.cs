using Microsoft.AspNetCore.Mvc;

using Web.Data;
using Web.Models;
using Web.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroceryListController : Controller
    {
        private readonly IGroceryFoodService _foodService;
        private readonly DataContext _dataContext;
        public GroceryListController(IGroceryFoodService foodService, DataContext dataContext) {
            _foodService = foodService;
            _dataContext = dataContext;
        
        
        }
        // GET: api/<GroceryListController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<GroceryListController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            string? json = await _foodService.ExportGroceryList(id);
        
            return Json(json);
        }

        // POST api/<GroceryListController>
        [HttpPost]
        public void Post([FromBody] string value)

        {

        }

        // PUT api/<GroceryListController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Value value)
        {
            await _foodService.ChangeQuantity(await _foodService.RetrieveGroceryItem(id), value.value);
        }

        //api/grocerylist/change/id

        [HttpPut("change/{id}")]
        public async Task ChangePut(string id)
        {
           var values =  await _dataContext.GroceryFood.FindAsync(id);
          
            values.InBasket = !values.InBasket;
            _dataContext.GroceryFood.Update(values);
            await _dataContext.SaveChangesAsync();
        }

        // DELETE api/<GroceryListController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}


