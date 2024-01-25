using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragAndDropController : MonoBehaviour
{
    [SerializeField] private float timeToDetectDrag;
    [SerializeField] private float distanceToDetectDrag;
    [SerializeField] private float overlapCircleRadius = 0.5f;
    [SerializeField] private ProductObject productObjectPrefab;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private float productMoveAnimationDuration = 0.3f;

    public event Action<bool> OnProductReleased;

    private Coroutine detectDragCoroutine;
    private bool isDragging;
    private ProductObject productObject;
    private Transform productTransform;
    private bool isActive = true;
    private Transform productOriginalPosTransform;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        if (Input.GetButtonDown("Touch"))
        {
            detectDragCoroutine = StartCoroutine(DetectDrag());
        }
        if (isDragging)
        {
            productTransform.SetPositionAndRotation(Input.mousePosition, Quaternion.identity);
        }

        if (Input.GetButtonUp("Touch"))
        {
            StopDrag();

            if (productObject != null)
            {
                OnReleaseProduct();
            }
        }
    }

    private void StopDrag()
    {
        if (detectDragCoroutine != null)
        {
            StopCoroutine(detectDragCoroutine);
            detectDragCoroutine = null;
        }

        isDragging = false;
    }

    public void ToggleIsActive()
    {
        isActive = !isActive;
        if (!isActive)
        {
            StopDrag();
        }
    }

    private IEnumerator DetectDrag()
    {
        Vector3 startingPosition = Input.mousePosition;

        for (float elapsedTime = 0; elapsedTime < timeToDetectDrag ; elapsedTime += Time.deltaTime)
        {
            yield return null;

            if (startingPosition.magnitude > distanceToDetectDrag)
            {
                break;
            }
        }

        bool gotProduct = TryGetProductFromContainer(out ProductWithState productData);

        if (!gotProduct)
        {
            yield break;
        }

        productObject = Instantiate(productObjectPrefab, Input.mousePosition, Quaternion.identity, canvasTransform);
        productObject.Setup(productData);
        productTransform = productObject.transform;

        isDragging = true;
    }

    private bool TryGetProductFromContainer(out ProductWithState productData)
    {
        productData = default;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(Input.mousePosition, overlapCircleRadius);

        if (hitColliders.Length == 0)
        {
            return false;
        }

        IProductContainer productContainer = null;
        
        int i = 0;
        for (; i < hitColliders.Length ; i++)
        {
            Collider2D collider = hitColliders[i];

            productContainer = collider.GetComponent<IProductContainer>();

            if (productContainer != null)
            {
                break;
            }

            if(i + 1 == hitColliders.Length)
            {
                return false;
            }
        }

        productOriginalPosTransform = hitColliders[i].transform;
        return productContainer.TryTakeProduct(out productData, out OnProductReleased);
    }

    public void OnReleaseProduct()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(Input.mousePosition, overlapCircleRadius);

        if (hitCollider == null)
        {
            MoveProductToOriginalPosAndDestroy();
            return;
        }

        var ingredientReceiver = hitCollider.GetComponent<IProductReceiver>();

        if (ingredientReceiver == null)
        {
            MoveProductToOriginalPosAndDestroy();
            return;
        }

        ingredientReceiver.ReceiveProduct(productObject.ProductAndState);
        OnProductReleased?.Invoke(true);
        DestroyProductInstance();
    }

    private void MoveProductToOriginalPosAndDestroy()
    {
        productObject.transform.DOMove(productOriginalPosTransform.position, productMoveAnimationDuration).onComplete += () =>
        {
            OnProductReleased?.Invoke(false);
            DestroyProductInstance();
        };
        
    }

    private void DestroyProductInstance()
    {
        Destroy(productObject.gameObject);
        productObject = null;
    }
}