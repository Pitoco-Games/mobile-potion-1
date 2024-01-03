public interface IProductContainer
{
    bool TryTakeProduct(out (ProductConfig, ProductState) productData);
}