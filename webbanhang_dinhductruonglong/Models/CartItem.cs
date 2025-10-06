using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webbanhang_dinhductruonglong.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; } 

        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int ShoppingCartId { get; set; } 
        [ForeignKey("ShoppingCartId")]
        public ShoppingCart ShoppingCart { get; set; }
    }
}
