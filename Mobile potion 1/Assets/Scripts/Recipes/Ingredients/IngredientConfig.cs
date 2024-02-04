using UnityEngine;

[CreateAssetMenu(menuName = "Potions/Ingredient", fileName = "Ingredient_NameOfIngredient")]
public class IngredientConfig : ProductConfig
{
    public int requiredMortarAndPestleStages;
    public Sprite mashedProgressSprite;
    public Sprite fullyMashedSprite;
}