using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronDrawingDetectionPattern : MonoBehaviour
{
    [SerializeField] private CauldronDrawingPointDetector[] drawingPoints;
    [SerializeField] private float nextPointDistanceThreshold = 1f;

    private Action<bool> onStoppedDrawing;

    private int numberOfPoints;
    private int nextPointIndex;
    private Transform nextPointTransform;
    private Camera mainCamera;
    private bool drawingSucceded;

    public void Setup(Action<bool> onStoppedDrawingCallback)
    {
        onStoppedDrawing += onStoppedDrawingCallback;
        mainCamera = Camera.main;

        numberOfPoints = drawingPoints.Length;
        ResetProgress();

        foreach (var point in drawingPoints)
        {
            point.SubscribeToCollisionDetection(IncrementDetectedPointsAndGetNext);
        }
    }

    private void Update()
    {
        if(!Input.GetButton("Touch"))
        {
            return;
        }

        float touchPosToNextPointDistance = Vector2.Distance(nextPointTransform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition));
        Debug.Log(touchPosToNextPointDistance);

        if (touchPosToNextPointDistance > nextPointDistanceThreshold)
        {
            drawingSucceded = false;
        }
    }

    private void IncrementDetectedPointsAndGetNext()
    {
        nextPointIndex++;
        if (nextPointIndex == numberOfPoints)
        {
            drawingSucceded = true;
            return;
        }

        nextPointTransform = drawingPoints[nextPointIndex].transform;
    }

    public void ResetProgress()
    {
        nextPointIndex = 0;
        nextPointTransform = drawingPoints[nextPointIndex].transform;
        drawingSucceded = false;

        foreach (var point in drawingPoints)
        {
            point.ResetDetection();
        }
    }

    public void FinishDrawing()
    {
        onStoppedDrawing.Invoke(drawingSucceded);
        ResetProgress();
    }
}