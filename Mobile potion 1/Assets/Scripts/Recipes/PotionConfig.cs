using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potions/Potion", fileName = "Potion_PotionName")]
public class PotionConfig : ProductConfig
{
    public List<ProductWithState> RequiredIngredients;
    public float TimeRequiredInCauldron;

    public void SortIngredientsList()
    {
        RequiredIngredients.Sort();
    }
}

[Serializable]
public struct ProductWithState
{
    public ProductConfig config;
    public ProductState state;
}

public enum ProductState {Raw, Mashed, Roasted, Extracted, Brewed}