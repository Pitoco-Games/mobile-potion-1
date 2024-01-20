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

    public event Action<bool> OnProductReleased;

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

        bool gotProduct = TryGetProductFromContainer(out ProductWithState productData);

        if (!gotProduct)
        {
            yield break;
        }

        productObject = Instantiate(productObjectPrefab, Input.mousePosition, Quaternion.identity, canvasTransform);
        productObject.Setup(productData);
        productRigidbody = productObject.GetComponent<Rigidbody2D>();

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

        for (int i = 0 ; i < hitColliders.Length ; i++)
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
        
        return productContainer.TryTakeProduct(out productData, out OnProductReleased);
    }

    public void OnReleaseProduct()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(Input.mousePosition, overlapCircleRadius);

        if (hitCollider == null)
        {
            DestroyProductInstance();
            OnProductReleased?.Invoke(false);
            return;
        }

        var ingredientReceiver = hitCollider.GetComponent<IProductReceiver>();

        if (ingredientReceiver == null)
        {
            DestroyProductInstance();
            OnProductReleased?.Invoke(false);
            return;
        }

        ingredientReceiver.ReceiveProduct(productObject.ProductAndState);
        OnProductReleased?.Invoke(true);
        DestroyProductInstance();
    }

    private void DestroyProductInstance()
    {
        Destroy(productObject.gameObject);
        productObject = null;
    }
}