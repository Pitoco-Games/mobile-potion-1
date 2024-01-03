using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IProductContainer
{
    [SerializeField] private Image ingredientImage;

    private IngredientConfig ingredient;

    public void Setup(IngredientConfig ingredient)
    {
        this.ingredient = ingredient;
        ingredientImage.sprite = ingredient.Sprite;
    }


    public bool TryTakeProduct(out (ProductConfig, ProductState) productData)
    {
        productData = (ingredient, ProductState.Raw);
        return true;
    }
}