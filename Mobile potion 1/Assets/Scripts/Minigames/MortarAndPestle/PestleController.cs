using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestleController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private Ease inEase;
    [SerializeField] private Ease outEase;

    public event Action OnIngredientCollision;

    private Vector2 originalPos;
    private bool isMoving = false;

    private void Awake()
    {
        originalPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var ingredient = collision.gameObject.GetComponent<MortarAndPestleIngredient>();

        if (ingredient != null && ingredient.CanDetectCollisions)
        {
            OnIngredientCollision?.Invoke();
        }
    }

    public void MoveToPositionAndReturn(Vector2 position)
    {
        if(isMoving)
        {
            return;
        }

        isMoving = true;

        Sequence moveSequence = DOTween.Sequence();

        moveSequence.Append(rigidbody.DOMove(position, moveDuration).SetEase(inEase));
        moveSequence.AppendInterval(0.1f);
        moveSequence.Append(rigidbody.DOMove(originalPos, moveDuration).SetEase(outEase));
        moveSequence.OnComplete(SetIsMovingFalse);

        moveSequence.Play();

        void SetIsMovingFalse()
        {
            isMoving = false;
        }
    }
}
