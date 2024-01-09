using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientController : MonoBehaviour
{
    [SerializeField] private List<ClientConfig> possibleClients = new();
    [SerializeField] private ClientView clientView;

    private ClientConfig currentClient;
    private OrderConfig currentOrder;

    private void Start()
    {
        ShowNextClient();
    }

    private void ShowNextClient()
    {
        currentClient = PickNextClient();
        currentOrder = currentClient.PickRandomOrder();

        clientView.SetupNewClient(currentClient, currentOrder);
    }

    public ClientConfig PickNextClient()
    {
        while(true)
        {
            ClientConfig candidate = possibleClients[Random.Range(0, possibleClients.Count)];

            bool pickedSameClientAsLast = currentClient != null && candidate.Name.Equals(currentClient.Name);
            
            if(!pickedSameClientAsLast)
            {
                return candidate;
            }
        }
    }

    public bool TryReceiveProduct(ProductWithState productData)
    {
        if (productData.config is PotionConfig potion)
        {
            if(potion.Name.Equals(currentOrder.requiredPotion.Name))
            {
                ShowNextClient();

                return true;
            }
        }

        return false;
    }
}
