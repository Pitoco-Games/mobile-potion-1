using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientInventory : MonoBehaviour
{
    [SerializeField] private List<IngredientConfig> availableIngredients;
    [SerializeField] private InventorySlot inventorySlotPrefab;
    [SerializeField] private Transform ingredientsParent;
    [SerializeField] private Button OpenAndCloseButton;
    [SerializeField] private RectTransform inventoryParentTransform;
    [SerializeField] private float openAndCloseAnimationDuration = 0.5f;
    [SerializeField] private Ease animationEase;

    private bool isOpen = false;
    private float inventoryClosedXPosition;

    private void Awake()
    {
        InstantiateIngredientSlots();
        inventoryClosedXPosition = inventoryParentTransform.anchoredPosition.x;
        OpenAndCloseButton.onClick.AddListener(ToggleOpenAndClosedState);
    }

    private void InstantiateIngredientSlots()
    {
        foreach (IngredientConfig ingredient in availableIngredients)
        {
            InventorySlot ingredientSlot = Instantiate(inventorySlotPrefab, ingredientsParent);

            ingredientSlot.Setup(ingredient);
        }
    }

    private void ToggleOpenAndClosedState()
    {
        float positionToAnimateTo;
        isOpen = !isOpen;

        positionToAnimateTo = isOpen ? 0 : inventoryClosedXPosition;

        inventoryParentTransform.DOAnchorPosX(positionToAnimateTo, openAndCloseAnimationDuration).SetEase(animationEase);
    }
}