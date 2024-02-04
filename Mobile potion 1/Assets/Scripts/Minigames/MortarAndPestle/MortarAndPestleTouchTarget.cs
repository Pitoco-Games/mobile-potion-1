using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MortarAndPestleTouchTarget : MonoBehaviour
{
    [SerializeField] private Button button;

    public void SubscribeToTouchEvent(UnityAction touchCallback)
    {
        button.onClick.AddListener(touchCallback);
    }

    public void MoveToPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}