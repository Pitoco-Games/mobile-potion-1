using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipes/Ingredient", fileName = "Ingredient_NameOfIngredient")]
public class IngredientConfig : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Sprite;
    public int requiredMortarAndPestleHits;
}