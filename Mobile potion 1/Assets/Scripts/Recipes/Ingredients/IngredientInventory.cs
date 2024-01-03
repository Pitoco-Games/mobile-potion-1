using System.Collections.Generic;
using UnityEngine;

public class IngredientInventory : MonoBehaviour
{
    [SerializeField] private List<IngredientConfig> availableIngredients;
    [SerializeField] private InventorySlot inventorySlotPrefab;
    [SerializeField] private Transform ingredientsParent;

    private void Awake()
    {
        InstantiateIngredientSlots();
    }

    private void InstantiateIngredientSlots()
    {
        foreach (IngredientConfig ingredient in availableIngredients)
        {
            InventorySlot ingredientSlot = Instantiate(inventorySlotPrefab, ingredientsParent);

            ingredientSlot.Setup(ingredient);
        }
    }
}