using EFarm.Server.Data;
using EFarm.Server.Model;
using EFarm.Server.Model.Dto;
using EFarm.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EFarm.Server.Controllers
{
    [Route("api/basket")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        readonly AppDbContext _dbContext;
        IAppSession _session;
         public BasketController(AppDbContext dbContext, IAppSession     session)
        {
           _dbContext = dbContext;
            _session = session;
            //basket = basketService.GetOrCreateBasketForUser().GetAwaiter().GetResult();
        }

        // GET: api/<BasketController>
        [HttpGet]
        public async Task<BasketDto> Get()
        {
            var entity = await GetOrCreateBasketForUser();
            var basket = new BasketDto
            {
                Id = entity.Id,
                BuyerId = entity.BuyerId,
            };
            foreach (var item in entity.Items)
            {
                basket.Items.Add(new BasketItemDto
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Product = new ProductDto
                    {
                        Name = item.Product?.Name,
                        PictureUri = item.Product?.PictureUri,
                    }
                });
            }
            return basket;


        }

        [HttpGet("count")]
        public async Task<int> CountBasketItems(){
            var basket = await _dbContext.Baskets.Include(x => x.Items)
                 .FirstOrDefaultAsync(basket => basket.BuyerId == _session.UserId);
            var totalItems = basket == null ? 0 : basket.Items.GroupBy(x => x.ProductId).Count();

            return totalItems;
        }

        [HttpPost("add")]
        public async Task AddItem([FromBody] BasketItemDto item)
        {
            var basket = await GetOrCreateBasketForUser();
            var items = await _dbContext.BasketItem.Where(x => x.BasketId == basket.Id).ToListAsync();
            if (!items.Any(i => i.ProductId == item.ProductId))
            {
                ;
                _dbContext.BasketItem.Add(new BasketItem(item.ProductId, item.Quantity, item.UnitPrice) { BasketId = basket.Id });
            }
            else
            {
                var existingItem = items.First(i => i.ProductId == item.ProductId);
                existingItem.AddQuantity(item.Quantity);
                _dbContext.BasketItem.Update(existingItem);
            }
            await _dbContext.SaveChangesAsync();
        }
       
        [HttpDelete("remove/{id}")]
        public async Task RemoveItem([FromRoute] int id)
        {
            var basket = await _dbContext.Baskets.FirstOrDefaultAsync(x => x.BuyerId == _session.UserId);
            if (basket is null) return;
            var item = await _dbContext.BasketItem.FirstOrDefaultAsync(x => x.Id == id && x.BasketId == basket.Id);
            if (item is null) return;
            _dbContext.Remove(item);
            await _dbContext.SaveChangesAsync();
        }
         
        [HttpPut("quantity")]
        public async Task UpdateQuantity([FromBody] BasketItemDto item)
        {
            var basket = await _dbContext.Baskets.FirstOrDefaultAsync(x => x.BuyerId == _session.UserId);
            if (basket is null) return;
            var basketItem = await _dbContext.BasketItem.FirstOrDefaultAsync(x => x.Id == item.Id &&
               x.BasketId == basket.Id);
            if (basketItem is null) return;
            basketItem.Quantity = item.Quantity;
            _dbContext.Update(basketItem);
            await _dbContext.SaveChangesAsync();
        }
 
        [HttpPost("checkout")]
        public async Task ClearBasket()
        {
            var basket = await _dbContext.Baskets.Include(x => x.Items).FirstOrDefaultAsync(x => x.BuyerId == _session.UserId);
            if (basket is null) return;

            _dbContext.BasketItem.RemoveRange(basket.Items);
            _dbContext.Baskets.Remove(basket);
            await _dbContext.SaveChangesAsync();
        }
 

        private async Task<Basket> GetOrCreateBasketForUser()
        {
            var userId = _session.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new Exception("No User found.");

            var basket = await GetUserBasket(userId);

            if (basket == null)
            {
                basket = await CreateBasketForUser(userId);
            }
            return basket;
        }

        private async Task<Basket> CreateBasketForUser(string userId)
        {
            var basket = new Basket(userId);
            await _dbContext.Baskets.AddAsync(basket);
            await _dbContext.SaveChangesAsync();

            return basket;
        }


        private async Task<Basket> GetUserBasket(string userId)
        {
            var basket = await _dbContext.Baskets.Include(x => x.Items).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.BuyerId == userId);
            return basket;
        }
    }

}
