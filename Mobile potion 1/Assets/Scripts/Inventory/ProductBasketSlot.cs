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

    public bool TryTakeProduct(out ProductWithState productData)
    {
        productData = default;

        if (containedProduct.config == null)
        {
            return false;
        }

        productData = containedProduct;
        containedProduct.config = null;
        productImage.gameObject.SetActive(false);

        return true;
    }
}
