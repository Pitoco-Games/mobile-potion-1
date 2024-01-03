using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProductObject : MonoBehaviour/*, IDragHandler, IEndDragHandler, IPointerClickHandler, IBeginDragHandler*/
{
    [SerializeField] private Image ingredientImage;
    [SerializeField] private Rigidbody2D rigidbody;

    public ProductConfig ProductConfig => product;
    public ProductState State => state;

    private ProductState state;
    private ProductConfig product;
    private Action onBeginDragCallback;
    private Collider2D latestInteractedCollider;

    public void Setup(ProductConfig product, ProductState state)
    {
        this.product = product;
        ingredientImage.sprite = product.Sprite;
        this.state = state;
    }

    /*public void OnDrag(PointerEventData eventData)
    {
        rigidbody.MovePosition(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (latestInteractedCollider == null)
        {
            Destroy(gameObject);
            return;
        }

        var ingredientReceiver = latestInteractedCollider.gameObject.GetComponent<IProductReceiver>();

        if (ingredientReceiver == null)
        {
            Destroy(gameObject);
            return;
        }

        if (ingredientReceiver.ReceiveProduct(this))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        latestInteractedCollider = other;
    }

    private void OnTriggerExit(Collider other)
    {
        latestInteractedCollider = null;
    }

    //TODO: Find a way to detect to show description only when giving a quick tap
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Ingredient: {product.Name} / Description: {product.Description}");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        onBeginDragCallback?.Invoke();
    }*/
}