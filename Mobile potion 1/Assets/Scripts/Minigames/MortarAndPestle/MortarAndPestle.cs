using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MortarAndPestle : MonoBehaviour, IProductReceiver, IProductContainer
{
    [SerializeField] private MortarAndPestleTouchTarget touchTarget;
    [SerializeField] private RectTransform areaLimiterRectTransform;
    [SerializeField] private GameObject popupGameObject;
    [SerializeField] private DragAndDropController dragAndDropController;
    [SerializeField] private Image ingredientImage;

    [SerializeField] private MortarAndPestleMinigame minigamePrefab;
    [SerializeField] private Transform minigameParent;
    [SerializeField] private CanvasGroup canvasGroup;

    private float areaRadius;
    Vector2 circleCenter;

    private IngredientConfig currentIngredient;
    private int currentHits;

    /*private void Awake()
    {
        touchTarget.SubscribeToTouchEvent(RegisterCorrectAction);   
    }

    private void SetupPossibleHitPositionParameters()
    {
        Vector2 rectSize = areaLimiterRectTransform.sizeDelta;
        areaRadius = Mathf.Min(rectSize.x, rectSize.y) / 2f;

        circleCenter = areaLimiterRectTransform.position;
    }*/

    public bool ReceiveProduct(ProductWithState product)
    {
        if (currentIngredient == null && product.state == ProductState.Raw && product.config is IngredientConfig config)
        {
            currentIngredient = config;
            MortarAndPestleMinigame minigame = Instantiate(minigamePrefab, minigameParent);
            minigame.StartMinigame(currentIngredient, OnGameEnded);
            canvasGroup.alpha = 0f;

            dragAndDropController.ToggleIsActive();

            return true;
        }

        return false;
    }

    /*private void StartMiniGame()
    {
        SetupPossibleHitPositionParameters();
        ShowMiniGamePopup();
        MoveTargetToNewPosition();
    }

    private void ShowMiniGamePopup()
    {
        popupGameObject.SetActive(true);
    }

    private void MoveTargetToNewPosition()
    {
        // Generate random polar coordinates within the circle
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float randomRadius = Random.Range(0f, areaRadius);

        // Convert polar coordinates to Cartesian coordinates
        float randomX = circleCenter.x + randomRadius * Mathf.Cos(randomAngle);
        float randomY = circleCenter.y + randomRadius * Mathf.Sin(randomAngle);

        touchTarget.MoveToPosition(new Vector2(randomX, randomY));
    }

    private void RegisterCorrectAction()
    {
        currentHits++;

        if (GameIsFinished())
        {
            currentHits = 0;

            OnGameEnded();
            return;
        }

        MoveTargetToNewPosition();
    }

    private bool GameIsFinished()
    {
        return currentHits > currentIngredient.requiredMortarAndPestleStages;
    }
    */

    private void OnGameEnded()
    {
        dragAndDropController.ToggleIsActive();
        popupGameObject.SetActive(false);
        ingredientImage.sprite = currentIngredient.Sprite;
        ingredientImage.gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
    }

    private void OnProductReleasedByDrag(bool wentToNewProductReceiver)
    {
        if(wentToNewProductReceiver)
        {
            currentIngredient = null;
            return;
        }

        ingredientImage.gameObject.SetActive(true);
    }

    public bool TryTakeProduct(out ProductWithState productData, out Action<bool> onProductReleased)
    {
        productData = default;
        onProductReleased = OnProductReleasedByDrag;

        if (currentIngredient == null)
        {
            return false;
        }

        ProductConfig currConfig = currentIngredient;
        productData = new ProductWithState { config = currConfig, state = ProductState.Mashed };
        ingredientImage.gameObject.SetActive(false);

        return true;
    }
}