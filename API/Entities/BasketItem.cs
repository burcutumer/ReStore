using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("BasketItems")]
    public class BasketItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

       
        public int ProductId { get; set; }
        public Product Product { get; set; } //nav prop ---dont have a full product inside BasketItem table JUST the productID

        public int BasketId { get; set; }
        public Basket Basket { get; set; } //nav prop ---dont have a full basket inside BasketItem table JUST the basketID
    }
}