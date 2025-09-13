using Core.entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IGenericRepository<Product> _repository, IProductRepository _prodRepo) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            return Ok(await _repository.ListAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null) return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _repository.Add(product);

            if (await _repository.SaveAllAsync())
            {
                return CreatedAtAction("GetProductById", new { id = product.Id }, product);
            }

            return BadRequest("Problem creating product!");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !_repository.Exists(id)) return BadRequest("Cannat update the product");

            _repository.Update(product);


            if (await _repository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem in updating product!");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null) return NotFound();

            _repository.Remove(product);

            if (await _repository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem in delete product!");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            //TO DO: Implement method

            // return Ok(await _repository.GetBrandsAsync());
            return Ok();
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            //TO DO: Implement method

            // return Ok(await _repository.GetTypesAsync());
            return Ok();
        }
    }
}
