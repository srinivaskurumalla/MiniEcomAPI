namespace MiniEcom.Dtos
{
    public class SearchResultDto
    {
        public List<ProductSummaryDto> Products { get; set; } = [];
        public List<string> Tags { get; set; } = [];
    }

    public class ProductSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ShortDescription { get; set; } 
        public string? ImageUrl { get; set; }
    }
}
