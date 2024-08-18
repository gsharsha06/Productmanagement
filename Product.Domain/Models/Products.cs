namespace Product.Domain.Models
{
    public class Products
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime AvailableFrom { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
