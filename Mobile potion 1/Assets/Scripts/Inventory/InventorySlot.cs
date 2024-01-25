using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IProductContainer
{
    [SerializeField] private Image ingredientImage;
    [SerializeField] private float takeAnimationScale = 0.4f;
    [SerializeField] private float takeAnimationDuration = 0.3f;
    [SerializeField] private float receiveAnimationRotation = 45f;
    [SerializeField] private float receiveAnimationDuration = 0.3f;

    private IngredientConfig ingredient;

    public void Setup(IngredientConfig ingredient)
    {
        this.ingredient = ingredient;
        ingredientImage.sprite = ingredient.Sprite;
    }

    public bool TryTakeProduct(out ProductWithState productData, out Action<bool> onProductReleased)
    {
        onProductReleased = OnProductReleased;
        productData = new ProductWithState { config = ingredient, state = ProductState.Raw };
        ingredientImage.transform.DOPunchScale(Vector2.one * takeAnimationScale, takeAnimationDuration);
        return true;
    }

    private void OnProductReleased(bool wentToNewProductReceiver)
    {
        if(!wentToNewProductReceiver)
        {
            ingredientImage.transform.DOPunchRotation(Vector3.forward * receiveAnimationRotation, receiveAnimationDuration);
        }
    }
}