using EcommerceApi.Domain.Entities;
using EcommerceApi.Application.DTO;

namespace EcommerceApi.Application.Interfaces;
public interface IProductService
{
    Task<ProductResponseDto> CreateProduct(CreateProductDto dto);
    
    Task<GetAllProductResponseDto> GetAllProduct(ProductRequestDto dto);
    Task<ProductResponseDto> GetSingleProduct(Guid id);
    Task<ProductResponseDto> UpdateProduct(Guid id, UpdateProductDto dto);
    Task DeleteProduct(Guid id);
    
    Task<int> ImportProductsFromExcelAsync(IFormFile file);
}