using UnityEngine;

public class CameraFollowSinglePlayer : MonoBehaviour
{
    public Transform target;

    [Header("Movement Settings")]
    public float smoothTime = 0.15f;
    private Vector3 currentVelocity = Vector3.zero;

    [Header("Zoom Settings")]
    public float zoom = 20;

    [Header("Y-Offset")]
    public float y_offset = 0f;

    private Camera cam;

    public void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = zoom;
        // Try to find them immediately on start
        FindTarget();
    }

    void LateUpdate()
    {
        if (target == null)
        {
            FindTarget();
            if (target == null) return;
        }
        Vector3 targetPos = new Vector3(target.position.x, target.position.y + y_offset, transform.position.z);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref currentVelocity,
            smoothTime
        );
    }

    // New Function to find players dynamically
    void FindTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("K");
        if (players.Length > 0) target = players[0].transform;
    }
}