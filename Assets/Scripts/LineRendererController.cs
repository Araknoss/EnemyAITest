using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer; 
    public void DrawLine(Vector3 startPosition, Vector3 endPosition)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
    }
    
    public void EndLineDraw()
    {
        lineRenderer.enabled = false;        
    }
}
