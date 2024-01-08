using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class MortarAndPestle : MonoBehaviour, IProductReceiver, IProductContainer
{
    [SerializeField] private MortarAndPestleTouchTarget touchTarget;
    [SerializeField] private RectTransform areaLimiterRectTransform;
    [SerializeField] private GameObject popupGameObject;
    [SerializeField] private DragAndDropController dragAndDropController;
    [SerializeField] private Image ingredientImage;

    private float areaRadius;
    Vector2 circleCenter;

    private IngredientConfig currentIngredient;
    private int currentHits;

    private void Awake()
    {
        touchTarget.SubscribeToTouchEvent(RegisterCorrectAction);

        SetupPossibleHitPositionParameters();
    }

    private void SetupPossibleHitPositionParameters()
    {
        Vector2 rectSize = areaLimiterRectTransform.rect.size;
        areaRadius = Mathf.Min(rectSize.x, rectSize.y) / 2f;

        circleCenter = areaLimiterRectTransform.position;
    }

    public bool ReceiveProduct(ProductWithState product)
    {
        if (currentIngredient == null && product.state == ProductState.Raw && product.config is IngredientConfig config)
        {
            currentIngredient = config;
            StartMiniGame();
            return true;
        }

        return false;
    }

    private void StartMiniGame()
    {
        ShowMiniGamePopup();
        MoveTargetToNewPosition();
        dragAndDropController.ToggleIsActive();
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

            EndGame();
            return;
        }

        MoveTargetToNewPosition();
    }

    private bool GameIsFinished()
    {
        return currentHits > currentIngredient.requiredMortarAndPestleHits;
    }

    private void EndGame()
    {
        dragAndDropController.ToggleIsActive();
        popupGameObject.SetActive(false);
        ingredientImage.sprite = currentIngredient.Sprite;
        ingredientImage.gameObject.SetActive(true);
    }

    public bool TryTakeProduct(out ProductWithState productData)
    {
        productData = default;

        if (currentIngredient == null)
        {
            return false;
        }

        ProductConfig currConfig = currentIngredient;
        currentIngredient = null;
        productData = new ProductWithState { config = currConfig, state = ProductState.Mashed };
        ingredientImage.gameObject.SetActive(false);

        return true;
    }
}