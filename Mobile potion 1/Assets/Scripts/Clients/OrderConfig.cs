using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Order_ClientNameOrderName", menuName = "Clients/Order")]
public class OrderConfig : ScriptableObject
{
    [TextArea(1, 4)]
    public string dialog;
    public PotionConfig requiredPotion;
}
