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

        [HttpDelete("/Shop/{id}")]
        public async Task<ShoppingCartDTO> Delete(int id)
        {
            return await new ShopEC().Delete(id);
        }

        [HttpGet("/CartItems/{activeCartID}")]
        public async Task<IEnumerable<ItemDTO>> GetCartItem(int activeCartID)
        {
            return await new ShopEC().GetCartItems(activeCartID);
        }
    }
}
