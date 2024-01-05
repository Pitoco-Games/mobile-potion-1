using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropController : MonoBehaviour
{
    [SerializeField] private float timeToDetectDrag;
    [SerializeField] private float distanceToDetectDrag;
    [SerializeField] private float overlapCircleRadius = 0.5f;
    [SerializeField] private ProductObject productObjectPrefab;
    [SerializeField] private Transform canvasTransform;

    private Coroutine detectDragCoroutine;
    private bool isDragging;
    private ProductObject productObject;
    private Rigidbody2D productRigidbody;
    private bool isActive = true;

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
        else if (isDragging)
        {
            productRigidbody.MovePosition(Input.mousePosition);
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

            //TODO: check if needs to change required distance on different device screen sizes
            if (startingPosition.magnitude > distanceToDetectDrag)
            {
                break;
            }
        }

        bool gotProduct = TryGetProductFromContainer(out (ProductConfig, ProductState) productData);

        if (!gotProduct)
        {
            yield break;
        }

        productObject = Instantiate(productObjectPrefab, Input.mousePosition, Quaternion.identity, canvasTransform);
        productObject.Setup(productData.Item1, productData.Item2);
        productRigidbody = productObject.GetComponent<Rigidbody2D>();

        isDragging = true;
    }

    private bool TryGetProductFromContainer(out (ProductConfig, ProductState) productData)
    {
        productData = default;

        Collider2D hitCollider = Physics2D.OverlapCircle(Input.mousePosition, overlapCircleRadius);

        if (hitCollider == null)
        {
            return false;
        }

        var productContainer = hitCollider.GetComponent<IProductContainer>();

        if (productContainer == null)
        {
            return false;
        }

        return productContainer.TryTakeProduct(out productData);
    }

    public void OnReleaseProduct()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(Input.mousePosition, overlapCircleRadius);

        // TODO: find way for product still be available when releasing it on top of nothing
        if (hitCollider == null)
        {
            DestroyProductInstance();
            return;
        }

        var ingredientReceiver = hitCollider.GetComponent<IProductReceiver>();

        // TODO: find way for product still be available when releasing it on top of nothing
        if (ingredientReceiver == null)
        {
            DestroyProductInstance();
            return;
        }

        // IMPORTANT: here it's supposed to destroy indeed
        ingredientReceiver.ReceiveProduct(productObject);
        DestroyProductInstance();
    }

    private void DestroyProductInstance()
    {
        Destroy(productObject.gameObject);
        productObject = null;
    }
}