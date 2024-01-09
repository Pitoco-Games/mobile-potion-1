using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientView : MonoBehaviour, IProductReceiver
{
    [SerializeField] private Image clientImage;
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private TMP_Text dialogTextObject;
    [SerializeField] private ClientController clientController;

    public void SetupNewClient(ClientConfig clientConfig, OrderConfig pickedOrder)
    {
        clientImage.sprite = clientConfig.clientSprite;
        speechBubble.SetActive(true);

        dialogTextObject.text = pickedOrder.dialog;
    }

    public bool ReceiveProduct(ProductWithState productData)
    {
        return clientController.TryReceiveProduct(productData);
    }
}
