using Microsoft.EntityFrameworkCore;
using ProductService.Domain;

namespace ProductService.DataAccess.EF;

public class ProductRepository : IProductRepository
{
    private readonly ProductDbContext productDbContext;

    public ProductRepository(ProductDbContext productDbContext)
    {
        this.productDbContext = productDbContext ?? throw new ArgumentNullException(nameof(productDbContext));
    }

    public async Task<Product> Add(Product product)
    {
        await productDbContext.Products.AddAsync(product);
        await productDbContext.SaveChangesAsync();
        return product;
    }

    public async Task<List<Product>> FindAllActive()
    {
        return await productDbContext
            .Products
            .Include(c => c.Covers)
            .Include("Questions.Choices")
            .Where(p => p.Status == ProductStatus.Active)
            .ToListAsync();
    }

    public async Task<Product> FindOne(string productCode)
    {
        return await productDbContext
            .Products
            .Include(c => c.Covers)
            .Include("Questions.Choices")
            .Where(p => p.Status == ProductStatus.Active)
            .FirstOrDefaultAsync(p => p.Code.ToUpper().Equals(productCode.ToUpper()));
    }

    public async Task<Product> FindById(long id)
    {
        return await productDbContext.Products.Include(c => c.Covers).Include("Questions.Choices")
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> ActiveProduct(long id)
    {
        var product = await productDbContext.Products.Include(c => c.Covers).FirstOrDefaultAsync(p => p.Id == id);
        product.Activate();
        await productDbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product> DiscomtinueProduct(long id)
    {
        var product = await productDbContext.Products.Include(c => c.Covers).FirstOrDefaultAsync(p => p.Id == id);
        product.Discontinue();
        await productDbContext.SaveChangesAsync();
        return product;
    }

    public async Task<List<Product>> FindAllDraft()
    {
        return await productDbContext
            .Products
            .Include(c => c.Covers)
            .Include("Questions.Choices")
            .Where(p => p.Status == ProductStatus.Draft)
            .ToListAsync();
    }

    public async Task<Product> Update(Product product)
    {
        var existingProduct = await productDbContext.Products.FindAsync(product.Id);

        if (existingProduct != null)
        {
            // Cập nhật các thuộc tính của existingProduct với giá trị từ updatedProduct
            productDbContext.Entry(existingProduct).CurrentValues.SetValues(product);
            await productDbContext.SaveChangesAsync();
        }

        return existingProduct;
    }
}