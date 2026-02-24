using UnityEngine;

public class TetherVisuals : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform entityA;
    public Transform entityB;

    void Update()
    {
        if (lineRenderer!=null && entityA != null && entityB != null)
        {
            lineRenderer.SetPosition(0, entityA.position);
            lineRenderer.SetPosition(1, entityB.position);
        } else
        {
            lineRenderer.enabled = false;
        }
    }
}