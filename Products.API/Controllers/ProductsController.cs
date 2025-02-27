using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.API.Dtos;
using Products.API.MessageBus;
using Products.API.Models;
using Products.API.Repositories;
using System.Text.Json;

namespace Products.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController(IProductRepo productRepo,
                                    IMapper mapper,
                                    IMessagePublisher messagePublisher) : ControllerBase
    {
        private readonly IProductRepo _productRepo = productRepo;
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] QueryParameters queryParams)
        {
            var products = await _productRepo.GetAllProducts(queryParams);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productRepo.GetProductById(id);
            return Ok(mapper.Map<ProductReadDto>(product));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = mapper.Map<Product>(productCreateDto);
            product.CreatedAt = DateTime.Now;

            await _productRepo.CreateProduct(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, mapper.Map<ProductReadDto>(product));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productRepo.GetProductById(id);

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Amount = dto.Amount;
            product.Description = dto.Description;

            await _productRepo.UpdateProduct(product);
            var eventDto = new EventDto
            {
                Id = Guid.NewGuid(),
                EventType = EventType.ProductUpdated,
                Data = JsonSerializer.Serialize(product)
            };
            await messagePublisher.PublishMessageAsync(eventDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _productRepo.DeleteProduct(id);
            return NoContent();
        }
    }
}
