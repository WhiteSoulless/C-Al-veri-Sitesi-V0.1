using SalesApp.Models.Shop;

namespace SalesApp.Services.Shop;

public class ProductService : IProductService
{
    private readonly List<Category> _categories = new()
    {
        new Category { Id = 1, Name = "Elektronik", Icon = "laptop", ProductCount = 12, ImageUrl = "https://picsum.photos/seed/electronics/400/300" },
        new Category { Id = 2, Name = "Moda & Giyim", Icon = "shirt", ProductCount = 24, ImageUrl = "https://picsum.photos/seed/fashion/400/300" },
        new Category { Id = 3, Name = "Ayakkabı", Icon = "shoe", ProductCount = 18, ImageUrl = "https://picsum.photos/seed/shoes/400/300" },
        new Category { Id = 4, Name = "Aksesuar & Saat", Icon = "watch", ProductCount = 9, ImageUrl = "https://picsum.photos/seed/watch/400/300" }
    };

    private readonly List<Product> _products = new()
    {
        new Product
        {
            Id = 1,
            Name = "Kablosuz Aktif Gürültü Engelleyici Kulaklık",
            Description = "30 saate kadar pil ömrü, üstün bas deneyimi ve aktif gürültü engelleme özelliği ile müziğin ritmini hissedin.",
            Price = 2499.00m,
            OriginalPrice = 3299.00m,
            ImageUrl = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=500&q=80",
            Rating = 4.8,
            ReviewCount = 342,
            CategoryId = 1,
            CategoryName = "Elektronik",
            IsFeatured = true,
            Stock = 15
        },
        new Product
        {
            Id = 2,
            Name = "Akıllı Titanyum Saat Pro",
            Description = "Safir cam, EKG ölçümü, 50 metre su geçirmezlik ve her zaman açık AMOLED ekran.",
            Price = 4199.99m,
            OriginalPrice = 4999.00m,
            ImageUrl = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=500&q=80",
            Rating = 4.9,
            ReviewCount = 189,
            CategoryId = 4,
            CategoryName = "Aksesuar & Saat",
            IsFeatured = true,
            Stock = 8
        },
        new Product
        {
            Id = 3,
            Name = "Ergonomik Spor Koşu Ayakkabısı",
            Description = "Nefes alabilen file yüzey, ultra hafif taban ve maksimum konfor sağlayan yastıklama sistemi.",
            Price = 1850.00m,
            OriginalPrice = 2200.00m,
            ImageUrl = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=500&q=80",
            Rating = 4.7,
            ReviewCount = 420,
            CategoryId = 3,
            CategoryName = "Ayakkabı",
            IsFeatured = true,
            Stock = 25
        },
        new Product
        {
            Id = 4,
            Name = "Klasik Deri Günlük Sırt Çantası",
            Description = "15 inç dizüstü bilgisayar bölmeli, suya dayanıklı hakiki deri el işçiliği çanta.",
            Price = 1290.00m,
            OriginalPrice = null,
            ImageUrl = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80",
            Rating = 4.6,
            ReviewCount = 74,
            CategoryId = 4,
            CategoryName = "Aksesuar & Saat",
            IsFeatured = false,
            Stock = 12
        },
        new Product
        {
            Id = 5,
            Name = "Premium Pamuklu Oversize Sweatshirt",
            Description = "%100 organik pamuk kumaş, yumuşak doku ve dökümlü modern kesim.",
            Price = 750.00m,
            OriginalPrice = 950.00m,
            ImageUrl = "https://images.unsplash.com/photo-1556905055-8f358a7a47b2?w=500&q=80",
            Rating = 4.5,
            ReviewCount = 115,
            CategoryId = 2,
            CategoryName = "Moda & Giyim",
            IsFeatured = true,
            Stock = 30
        }
    };

    public Task<List<Category>> GetCategoriesAsync()
    {
        return Task.FromResult(_categories);
    }

    public Task<List<Product>> GetFeaturedProductsAsync()
    {
        return Task.FromResult(_products.Where(p => p.IsFeatured).ToList());
    }

    public Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        return Task.FromResult(_products.Where(p => p.CategoryId == categoryId).ToList());
    }

    public Task<List<Product>> SearchProductsAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Task.FromResult(_products);

        var filtered = _products.Where(p =>
            p.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            p.CategoryName.Contains(query, StringComparison.OrdinalIgnoreCase)
        ).ToList();

        return Task.FromResult(filtered);
    }

    public Task<Product?> GetProductByIdAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }
}
