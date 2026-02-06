using EcommerceApi.Domain.Entities;
using EcommerceApi.Infrastructure.Data;
using EcommerceApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Application.DTO;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Application.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }
    
    
    // create product
    [HttpPost]
    public async Task<ProductResponseDto> CreateProduct(CreateProductDto data)
    {
        var product = new Product
        {
            Name = data.Name,
            Price = data.Price,
            Stock = data.Stock,
            Category =  data.Category,
            Description = data.Description,
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
            Description = product.Description,
            Category = product.Category
        };
    }
    
    // get only single product
    [HttpGet("{id:guid}")]
    public async Task<ProductResponseDto> GetSingleProduct(Guid id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(P => P.Id == id && P.IsAvailable);
        if (product == null)
        {
            throw new ArgumentException("Product not found");
        }

        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
            Description = product.Description,
            Category = product.Category
        };
    }
    
    public async Task<GetAllProductResponseDto> GetAllProduct(ProductRequestDto query)
    {
        var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
        var pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

        var productQuery = _context.Products
            .AsNoTracking()
            .Where(p => p.IsAvailable)
            .AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.Category))
        {
            productQuery = productQuery.Where(p => p.Category == query.Category);
        }

        var totalCount = await productQuery.CountAsync();
        var products = await productQuery
            .OrderByDescending(p => p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                Category = p.Category
            })
            .ToListAsync();

        return new GetAllProductResponseDto
        {
            Products = products,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }


    
    // update product
    [HttpPut("{id:guid}")]
    public async Task<ProductResponseDto> UpdateProduct(Guid id, UpdateProductDto data)
    {
        var product = await _context.Products.FirstOrDefaultAsync(P => P.Id == id);
        if (product == null)
        {
            throw new ArgumentException("Product not found");
        }
        
        product.Name =  data.Name;
        product.Category = data.Category;
        product.Price = data.Price;
        product.Description = data.Description;
        product.Stock = product.Stock;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
            Description = product.Description,
            Category = product.Category
        };
    }

    // delete product
    [HttpDelete("{id:guid}")]
    public async Task DeleteProduct(Guid id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(P => P.Id == id);
        if (product == null)
        {
            throw new ArgumentException("Product not found");
        }
        
        product.IsAvailable = false;
        await _context.SaveChangesAsync();
    }
}