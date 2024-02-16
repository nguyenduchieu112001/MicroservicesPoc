namespace ProductService.Domain;

public interface IProductRepository
{
    Task<Product> Add(Product product);

    Task<Product> Update(Product product);

    Task<List<Product>> FindAllActive();

    Task<List<Product>> FindAllDraft();

    Task<Product> FindOne(string productCode);

    Task<Product> FindById(long id);

    Task<Product> ActiveProduct(long id);

    Task<Product> DiscomtinueProduct(long id);
}