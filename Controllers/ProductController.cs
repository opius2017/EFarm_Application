using EFarm.Server.Data;
using EFarm.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EFarm.Server.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
      readonly  AppDbContext _dbContext;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        // GET: api/<ProductController>
        [HttpGet]
        public async Task<List<Product>> Get()
        {
            var products =await _dbContext.Products.ToListAsync();
            //if(!string.IsNullOrWhiteSpace(q))
            //    products=products.Where(x=>x.Name.ToLower().Contains(q.ToLower()));
            return products;
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<Product> Get(int id) => await _dbContext.Products.FindAsync(id);

    }
}
