using System;
using System.Collections.Generic;
using UnityEngine;

public class DrawingMinigame : MonoBehaviour
{
    [SerializeField] private Transform drawingPatternParent;
    [SerializeField] private float startingEnergy;
    [SerializeField] private float energyDrainSpeed = 5f;
    [SerializeField] private LineDrawer lineDrawerPrefab;
    [SerializeField] private SpriteRenderer ingredientSpriteRenderer;

    private DrawingDetectionPattern drawingDetectionPattern;
    private bool canDetectTouch;
    private LineDrawer currentLineDrawer;
    private Camera mainCamera;
    private Action onMinigameComplete;

    public void StartMinigame(IngredientConfig ingredientConfig, Action onMinigameComplete)
    {
        this.onMinigameComplete = onMinigameComplete;

        mainCamera = Camera.main;

        ingredientSpriteRenderer.sprite = ingredientConfig.Sprite;

        drawingDetectionPattern = Instantiate(ingredientConfig.drawingPatternPrefab, drawingPatternParent);
        drawingDetectionPattern.Setup(OnStoppedDrawing);
        canDetectTouch = true;
    }

    private void Update()
    {
        if(!canDetectTouch)
        {
            return;
        }

        if (Input.GetButtonDown("Touch"))
        {
            currentLineDrawer = Instantiate(lineDrawerPrefab);
        }

        if(Input.GetButtonUp("Touch"))
        {
            drawingDetectionPattern.FinishDrawing();
        }

        if(currentLineDrawer != null)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            currentLineDrawer.MoveColliderPosition(mousePos);
            currentLineDrawer.UpdateLine(mousePos);
        }
    }

    private void OnStoppedDrawing(bool completedDrawing)
    {
        drawingDetectionPattern.ResetProgress();
        currentLineDrawer.EraseLine();

        if (completedDrawing) 
        {
            canDetectTouch = false;
            FinishMinigame();
        }
        else
        {
            currentLineDrawer = null;
        }
    }

    private void FinishMinigame()
    {
        Destroy(drawingDetectionPattern.gameObject);
        Destroy(currentLineDrawer.gameObject);

        onMinigameComplete.Invoke();
        Destroy(gameObject);
    }
}
