using System;

public interface IProductContainer
{
    bool TryTakeProduct(out ProductWithState productData, out Action<bool> onProductReleased);
}