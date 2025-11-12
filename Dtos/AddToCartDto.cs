using System.ComponentModel.DataAnnotations;

namespace MiniEcom.Api.Dtos
{
    public class AddToCartDto
    {
      
        [Required] public int ProductId { get; set; }
        [Required] public int Quantity { get; set; }
    }
}
