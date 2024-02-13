using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Rigidbody2D colliderRigidbody;

    private List<Vector2> points;

    public void MoveColliderPosition(Vector2 position)
    {
        colliderRigidbody.MovePosition(position);
    }

    public void UpdateLine(Vector2 position)
    {
        if(points == null)
        {
            points = new List<Vector2>();
            SetPoint(position);
            return;
        }

        if (Vector2.Distance(position, points[^1]) > 0.1f)
        {
            SetPoint(position);
        }
    }

    public void EraseLine()
    {
        lineRenderer.material.DOFade(0, 0.5f).onComplete += () => Destroy(gameObject);
    }

    private void SetPoint(Vector2 point)
    {
        points.Add(point);

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
    }
}
