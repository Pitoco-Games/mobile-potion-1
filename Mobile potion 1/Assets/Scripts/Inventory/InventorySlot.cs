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


    public bool TryTakeProduct(out ProductWithState productData)
    {
        productData = new ProductWithState { config = ingredient, state = ProductState.Raw };
        return true;
    }
}