using System;
using System.Collections.Generic;
using UnityEngine;

public class CauldronMinigame : MonoBehaviour
{
    [SerializeField] private Transform drawingPatternParent;
    [SerializeField] private float startingEnergy;
    [SerializeField] private float energyDrainSpeed = 5f;
    [SerializeField] private LineDrawer lineDrawerPrefab;

    private CauldronDrawingDetectionPattern drawingDetectionPattern;
    private bool canDetectTouch;
    private List<GameObject> drawnLines = new ();
    private LineDrawer currentLineDrawer;
    private Camera mainCamera;
    private Action onMinigameComplete;

    public void StartMinigame(PotionConfig potionConfig, Action onMinigameComplete)
    {
        this.onMinigameComplete = onMinigameComplete;

        mainCamera = Camera.main;

        drawingDetectionPattern = Instantiate(potionConfig.cauldronDrawingPatternPrefab, drawingPatternParent);
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
            drawnLines.Add(currentLineDrawer.gameObject);
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
        if(completedDrawing) 
        {
            canDetectTouch = false;
            FinishMinigame();
        }
        else
        {
            drawingDetectionPattern.ResetProgress();
            currentLineDrawer.EraseLine();
            currentLineDrawer = null;
        }
    }

    private void FinishMinigame()
    {
        onMinigameComplete.Invoke();
        Destroy(gameObject);
    }
}
