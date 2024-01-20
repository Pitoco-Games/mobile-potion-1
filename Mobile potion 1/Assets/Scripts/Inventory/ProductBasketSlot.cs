using System;
using UnityEngine;
using UnityEngine.UI;

public class ProductBasketSlot : MonoBehaviour, IProductContainer, IProductReceiver
{
    [SerializeField] private Image productImage;
    private ProductWithState containedProduct;

    public bool ReceiveProduct(ProductWithState productData)
    {
        if(containedProduct.config != null)
        {
            return false;
        }

        containedProduct = productData;
        productImage.gameObject.SetActive(true);
        productImage.sprite = productData.config.Sprite;
        return true;
    }

    private void OnProductReleasedByDrag(bool wentToNewProductReceiver)
    {
        if (wentToNewProductReceiver)
        {
            containedProduct.config = null;
            return;
        }

        productImage.gameObject.SetActive(true);
    }

    public bool TryTakeProduct(out ProductWithState productData, out Action<bool> onProductReleased)
    {
        productData = default;
        onProductReleased = OnProductReleasedByDrag;

        if (containedProduct.config == null)
        {
            return false;
        }

        productData = containedProduct;
        productImage.gameObject.SetActive(false);

        return true;
    }
}
