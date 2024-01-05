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
public struct ProductWithState: IComparable
{
    public ProductConfig config;
    public ProductState state;

    public int CompareTo(object obj)
    {
        var otherConfig = (ProductWithState)obj;

        return config.Name.CompareTo(otherConfig.config.Name);
    }
}

public enum ProductState {Raw, Mashed, Roasted, Extracted, Brewed}