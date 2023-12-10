using UnityEngine;
using UnityEngine.UI;

public class MortarAndPestle : MonoBehaviour
{
    [SerializeField] private MortarAndPestleTouchTarget touchTarget;
    [SerializeField] private RectTransform areaLimiterRectTransform;
    [SerializeField] private Button startMiniGameButton;
    [SerializeField] private GameObject popupGameObject;
    [SerializeField] private int minimumHits = 3;
    [SerializeField] private int maximumHits = 6;

    private float areaRadius;
    Vector2 circleCenter;

    private int hitsRequired;
    private int currentHits;

    private void Awake()
    {
        startMiniGameButton.onClick.AddListener(StartMiniGame);
        touchTarget.SubscribeToTouchEvent(RegisterCorrectAction);

        SetupPossibleHitPositionParameters();
    }

    private void SetupPossibleHitPositionParameters()
    {
        Vector2 rectSize = areaLimiterRectTransform.rect.size;
        areaRadius = Mathf.Min(rectSize.x, rectSize.y) / 2f;

        circleCenter = areaLimiterRectTransform.position;
    }

    private void StartMiniGame()
    {
        SetupRequiredHitsAmount();
        ShowMiniGamePopup();
        MoveTargetToNewPosition();
    }

    private void SetupRequiredHitsAmount()
    {
        hitsRequired = Random.Range(minimumHits, maximumHits + 1);
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
        return currentHits > hitsRequired;
    }

    private void EndGame()
    {
        popupGameObject.SetActive(false);
    }
}