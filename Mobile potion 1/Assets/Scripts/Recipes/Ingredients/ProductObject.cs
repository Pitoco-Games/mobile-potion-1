using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProductObject : MonoBehaviour
{
    [SerializeField] private Image productImage;
    [SerializeField] private Rigidbody2D rigidbody;

    public ProductConfig ProductConfig => productAndState.config;
    public ProductState State => productAndState.state;
    public ProductWithState ProductAndState => productAndState;

    private ProductWithState productAndState;

    public void Setup(ProductWithState productData)
    {
        productAndState = productData;
        
        productImage.sprite = productAndState.config.Sprite;
    }
}