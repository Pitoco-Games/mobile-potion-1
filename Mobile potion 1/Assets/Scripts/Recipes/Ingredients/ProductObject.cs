using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProductObject : MonoBehaviour
{
    [SerializeField] private Image productImage;
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
        productImage.sprite = product.Sprite;
        this.state = state;
    }
}