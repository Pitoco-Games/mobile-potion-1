public interface IProductContainer
{
    bool TryTakeProduct(out ProductWithState productData);
}