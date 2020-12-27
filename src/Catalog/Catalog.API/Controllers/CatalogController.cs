using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController: ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id}", Name = "GetProduct")]
        [Route("GetProduct/{id}")]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var result = await _repository.GetProduct(id);
            if (id == null)
            {
                _logger.LogError($"{id} not found");
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetProductByCategory/{categoryName}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string categoryName)
        {
            var result = await _repository.GetProductByCategory(categoryName);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetProductByName/{name}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
        {
            var result = await _repository.GetProductByCategory(name);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            await _repository.Create(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _repository.Update(product));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProductById(string id)
        {

            return Ok(await _repository.Delete(id));
        }

    }
}
