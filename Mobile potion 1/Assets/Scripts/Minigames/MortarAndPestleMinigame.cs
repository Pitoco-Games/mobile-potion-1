using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarAndPestleMinigame : MonoBehaviour
{
    [SerializeField] private PestleController pestleController;

    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if(Input.GetButtonDown("Touch"))
        {
            Vector3 worldPos = camera.ScreenToWorldPoint(Input.mousePosition);
            pestleController.MoveToPositionAndReturn(worldPos);
        }
    }
}
