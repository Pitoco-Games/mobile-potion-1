using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenController : MonoBehaviour
{
    [SerializeField] private Button goToLabButton;
    [SerializeField] private Button goToCounterButton;
    [SerializeField] private RectTransform canvas;
    [SerializeField] RectTransform screensParentRectTransform;
    [SerializeField] private float changeScreenAnimationDuration = 0.5f;

    private float counterScreenXPosition;
    private float labScreenXPosition;
    bool hasCounterSelected = true;

    void Awake()
    {
        counterScreenXPosition = 0;
        labScreenXPosition = -canvas.sizeDelta.x;
        goToLabButton.onClick.AddListener(ToggleCurrentScreenSelection);
        goToCounterButton.onClick.AddListener(ToggleCurrentScreenSelection);
    }

    private void ToggleCurrentScreenSelection()
    {
        float positionToAnimateTo;
        hasCounterSelected = !hasCounterSelected;

        positionToAnimateTo = hasCounterSelected ? counterScreenXPosition : labScreenXPosition;

        screensParentRectTransform.DOAnchorPosX(positionToAnimateTo, changeScreenAnimationDuration);
    }
}
