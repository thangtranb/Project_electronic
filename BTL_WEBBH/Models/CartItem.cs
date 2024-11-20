using System.Drawing;

namespace BTL_WEBBH.Models
{
    public class CartItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total
        {
            get { return Price * Quantity; }
        }

        public string Image {  get; set; }
        public CartItem() { }
        public CartItem(Product product) {
            ProductId = product.Id;
            ProductName = product.ProName;
            Price = product.Price;
            Quantity = 1;
            Image = product.Image;
        }
    }
}
