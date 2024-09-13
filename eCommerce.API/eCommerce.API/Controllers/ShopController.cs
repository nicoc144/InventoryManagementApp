using eCommerce.API.EC;
using Microsoft.AspNetCore.Mvc;
using WebStore.Library.DTO;

namespace eCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShopController : ControllerBase
    {
        private readonly ILogger<ShopController> _logger; //readonly access to _logger
        public ShopController(ILogger<ShopController> logger) //dependency injection of ILogger
        {
            _logger = logger;
        }

        [HttpGet()]
        public async Task<IEnumerable<ShoppingCartDTO>> Get()
        {
            return await new ShopEC().Get();
        }

        [HttpPost()]
        public async Task<ShoppingCartDTO> AddOrUpdate([FromBody] ShoppingCartDTO p) //FromBody takes in a string and converts it to ItemDTO
        {
            return await new ShopEC().AddOrUpdate(p);
        }

        [HttpPost("{id}")] //Need to send in an id because we need to know what the user's active cart is
        public async Task<ItemDTO> AddOrUpdateItem([FromBody] ItemDTO i, int id)
        {
            return await new ShopEC().AddItemToCart(i, id);
        }

        [HttpPost("/DeleteCartItem/{id}")]
        public async Task<ItemDTO> DeleteOrReduceItem([FromBody] ItemDTO i, int id)
        {
            return await new ShopEC().DeleteOrReduceItem(i, id);
        }

        [HttpDelete("{id}")]
        public async Task<ShoppingCartDTO> Delete(int id)
        {
            return await new ShopEC().Delete(id);
        }
    }
}
