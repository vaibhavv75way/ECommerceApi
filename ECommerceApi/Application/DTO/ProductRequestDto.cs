namespace EcommerceApi.Application.DTO;
public class ProductRequestDto
{
    public string Category { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}