using System;
using UnityEngine;

public class CauldronDrawingPointDetector : MonoBehaviour
{
    private Action onCollisionDetected;
    private bool alreadyDetected;

    public void SubscribeToCollisionDetection(Action callback)
    {
        onCollisionDetected += callback;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(alreadyDetected) 
        { 
            return; 
        }

        onCollisionDetected?.Invoke();
        alreadyDetected = true;
    }

    public void ResetDetection()
    {
        alreadyDetected = false;
    }
}
