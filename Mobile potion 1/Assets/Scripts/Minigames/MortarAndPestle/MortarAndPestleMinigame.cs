using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MortarAndPestleMinigame : MonoBehaviour
{
    [SerializeField] private PestleController pestleController;
    [SerializeField] private MortarAndPestleIngredient ingredientPrefab;
    [SerializeField] private Transform ingredientSpawnTransform;
    [SerializeField] private IngredientConfig selectedIngredientConfig;

    public event Action OnGameEnded;

    private Camera camera;
    private int ingredientMashCount;
    private int requiredTotalCount = 0;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void Start()
    {
        pestleController.OnIngredientCollision += IncrementMashedIngredientCounter;
    }

    public void StartMinigame(IngredientConfig ingredientConfig, Action onGameEndedCallback)
    {
        OnGameEnded += onGameEndedCallback;

        for (int i = 0 ; i < ingredientConfig.requiredMortarAndPestleStages ; i++)
        {
            requiredTotalCount += (int)Mathf.Pow(2, i);
        }

        selectedIngredientConfig = ingredientConfig;
        MortarAndPestleIngredient ingredient = Instantiate(ingredientPrefab, ingredientSpawnTransform.position, Quaternion.identity);

        ingredient.Setup(ingredientConfig, 0, ingredient.transform.localScale);
    }

    private void IncrementMashedIngredientCounter()
    {
        ingredientMashCount++;

        if(ingredientMashCount >= requiredTotalCount)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        OnGameEnded?.Invoke();

        Destroy(gameObject);
    }


    private void Update()
    {
        if (Input.GetButtonDown("Touch"))
        {
            Vector3 worldPos = camera.ScreenToWorldPoint(Input.mousePosition);
            pestleController.MoveToPositionAndReturn(worldPos);
        }
    }
}
