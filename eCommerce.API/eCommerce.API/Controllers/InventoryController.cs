using eCommerce.API.EC;
using Microsoft.AspNetCore.Mvc;
using WebStore.Library.DTO;
using WebStore.Library.Models;

namespace eCommerce.API.Controllers
{

    [ApiController] //specifies to swagger that this is an api endpoint
    [Route("[controller]")] //route to "localhost:7244/WeatherForecast" for example, "controller" in square brackets means remove that substring from the route "localhost:7244/WeatherForecastController"
    public class InventoryController : ControllerBase //this is what gets called from the service proxy, this handled the user's requests
    {
        private readonly ILogger<InventoryController> _logger; //readonly access to _logger
        public InventoryController(ILogger<InventoryController> logger) //dependency injection of ILogger
        {
            _logger = logger;
        }

        [HttpGet()]
        public async Task<IEnumerable<ItemDTO>> Get()
        {
            return await new InventoryEC().Get();
        }

        [HttpPost()]
        public async Task<ItemDTO> AddOrUpdate([FromBody] ItemDTO p) //FromBody takes in a string and converts it to ItemDTO
        {
            return await new InventoryEC().AddOrUpdate(p);
        }

        //[HttpGet("Delete/{id}")] //it's possible to use this for delete, but not ideal because it causes security concerns
        //like allowing people to use the url to delete things out of the database

        [HttpDelete("{id}")] 
        public async Task<ItemDTO> Delete(int id)
        {
            return await new InventoryEC().Delete(id);
        }
    }
}
