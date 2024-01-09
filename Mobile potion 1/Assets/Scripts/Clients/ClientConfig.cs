using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Client_Name", menuName = "Clients/Client")]
public class ClientConfig : ScriptableObject
{
    public string Name;
    public List<OrderConfig> possibleOrders;
    public Sprite clientSprite;

    public OrderConfig PickRandomOrder()
    {
        return possibleOrders[Random.Range(0, possibleOrders.Count)];
    }
}
