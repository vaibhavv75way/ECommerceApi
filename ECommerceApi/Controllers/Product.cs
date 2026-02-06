using EcommerceApi.Application.DTO;
using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Application.Interfaces;

namespace EcommerceApi.Controllers;
[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("Id:Guid")]
    public async Task<IActionResult> GetSingleProduct(Guid id)
    {
        try
        {
            var product = await _productService.GetSingleProduct(id);
            return Ok(product);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct(
        [FromBody] CreateProductDto data)
    {
        try
        {
            var createdProduct = await _productService.CreateProduct(data);
            return Ok(createdProduct);
        }
        catch (Exception e)
        {
            return  BadRequest(e.Message);
        }
        
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] ProductRequestDto query)
    {
        try
        {
            var result = await _productService.GetAllProduct(query);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto data)
    {
        try
        {
            var updatedProduct = await _productService.UpdateProduct(id, data);
            return Ok(updatedProduct);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        try
        {
            await _productService.DeleteProduct(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}