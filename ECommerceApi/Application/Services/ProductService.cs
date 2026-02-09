using EcommerceApi.Domain.Entities;
using EcommerceApi.Infrastructure.Data;
using EcommerceApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Application.DTO;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

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
    
    public async Task<int> ImportProductsFromExcelAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Excel file is required");

        var products = new List<Product>();

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        var package = new ExcelPackage(stream);
        var worksheet = package.Workbook.Worksheets[0];
        var rowCount = worksheet.Dimension.Rows;

        for (int row = 2; row <= rowCount; row++)
        {
            var product = new Product
            {
                Name = worksheet.Cells[row, 1].Text,
                Description = worksheet.Cells[row, 2].Text,
                Price = decimal.Parse(worksheet.Cells[row, 3].Text),
                Category = worksheet.Cells[row, 4].Text,
                Stock = int.Parse(worksheet.Cells[row, 5].Text)
            };

            products.Add(product);
        }

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        return products.Count;
    }
}