using SalesApp.Models.Shop;

namespace SalesApp.Services.Shop;

public interface IProductService
{
    Task<List<Category>> GetCategoriesAsync();
    Task<List<Product>> GetFeaturedProductsAsync();
    Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
    Task<List<Product>> SearchProductsAsync(string query);
    Task<Product?> GetProductByIdAsync(int id);
}
