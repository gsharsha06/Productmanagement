using Product.Domain.Models;

namespace Product.Domain.Interfaces.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<Products>> GetAllProductsAsync();
        Task<Products> GetProductByIdAsync(Guid id);
        Task<Products> CreateProductAsync(Products product);
        Task<Products> UpdateProductAsync(Products product);
        Task DeleteProductAsync(Guid id);
    }
}
