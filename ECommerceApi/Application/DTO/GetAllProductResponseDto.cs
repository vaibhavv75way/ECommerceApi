namespace EcommerceApi.Application.DTO;
public class GetAllProductResponseDto
{
    public List<ProductResponseDto> Products { get; set; } = [];
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
