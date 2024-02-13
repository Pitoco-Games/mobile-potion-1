using System;
using UnityEngine;
using UnityEngine.UI;

public class MagicExtractor : MonoBehaviour, IProductReceiver, IProductContainer
{
    [SerializeField] private DrawingMinigame minigamePrefab;
    [SerializeField] private GameObject canvasObject;
    [SerializeField] private Image ingredientImage;
    [SerializeField] private Transform minigameParent;

    private ProductWithState currentProduct;

    /*private void Awake()
    {
        currentProduct = new ProductWithState { config = null, state = ProductState.Raw };
    }*/

    public bool TryTakeProduct(out ProductWithState productData, out Action<bool> onProductReleased)
    {
        productData = default;

        if(currentProduct.config == null)
        {
            onProductReleased = null;

            return false;
        }

        onProductReleased = OnProductReleasedByDrag;
        if(currentProduct.state != ProductState.Extracted)
        {
            currentProduct = new ProductWithState { config = currentProduct.config, state = ProductState.Extracted };
        }

        productData = currentProduct;
        ingredientImage.gameObject.SetActive(false);


        return true;
    }

    public bool ReceiveProduct(ProductWithState productData)
    {
        if(currentProduct.config != null)
        {
            return false;
        }

        currentProduct = productData;

        canvasObject.SetActive(false);

        DrawingMinigame minigame = Instantiate(minigamePrefab, minigameParent);
        minigame.StartMinigame((IngredientConfig)productData.config, OnMinigameComplete);

        return true;
    }

    private void OnMinigameComplete()
    {
        canvasObject.SetActive(true);
        ingredientImage.gameObject.SetActive(true);

        ingredientImage.sprite = currentProduct.config.Sprite;
    }

    private void OnProductReleasedByDrag(bool wentToNewProductReceiver)
    {
        if (wentToNewProductReceiver)
        {
            currentProduct.config = null;
            
            return;
        }

        ingredientImage.gameObject.SetActive(true);
    }
}
