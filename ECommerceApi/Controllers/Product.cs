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

    [HttpGet("id:Guid")]
    public async Task<IActionResult> GetSingleProduct(Guid id)
    {
        var product = await _productService.GetSingleProduct(id);
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(
        [FromBody] CreateProductDto data)
    {
        var createdProduct = await _productService.CreateProduct(data);
        return Ok(createdProduct);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] ProductRequestDto query)
    {
        var result = await _productService.GetAllProduct(query);
        return Ok(result);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto data)
    {
        var updatedProduct = await _productService.UpdateProduct(id, data);
        return Ok(updatedProduct);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        await _productService.DeleteProduct(id);
        return Ok();
    }

    [HttpPost("upload-excel")]
    public async Task<IActionResult> UploadExcel(IFormFile file)
    {
        var count = await _productService.ImportProductsFromExcelAsync(file);

        return Ok(new
        {
            Message = "Excel data imported successfully",
            Count = count
        });
    }
}