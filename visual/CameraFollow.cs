using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetA;
    public Transform targetB;

    [Header("Movement Settings")]
    public float smoothTime = 0.1f;
    private Vector3 currentVelocity = Vector3.zero;

    public bool loggingEnabled = false;

    [Header("Zoom Settings")]
    public float minZoom = 8f;
    public float maxZoom = 16f;
    public float zoomSensitivity = 0.5f;
    public float zoomSpeed = 5f;
    public float bias = 0.5f;

    public Camera cam;

    public void Start()
    {
        if (cam == null) cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (targetA == null || targetB == null)
        {
            FindTargets();
            if (targetA == null || targetB == null) return;
        }

        Vector3 midpoint = Vector3.Lerp(targetA.position, targetB.position, bias);
        if (loggingEnabled) Debug.Log($"CameraFollow Midpoint: {midpoint}");

        Vector3 targetPos = new Vector3(midpoint.x, midpoint.y, cam.gameObject.transform.position.z);
        if (loggingEnabled) Debug.Log($"CameraFollow Targetpos: {midpoint}");

        cam.gameObject.transform.position = Vector3.SmoothDamp(
            cam.gameObject.transform.position,
            targetPos,
            ref currentVelocity,
            smoothTime
        );
        if (loggingEnabled) Debug.Log($"CameraFollow new position: {cam.gameObject.transform.position}");

        float distance = Vector2.Distance(targetA.position, targetB.position);

        float targetOrthoSize = Mathf.Clamp(distance * zoomSensitivity + minZoom, minZoom, maxZoom);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetOrthoSize, Time.deltaTime * zoomSpeed);
    }

    void FindTargets()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length >= 2)
        {
            targetA = players[0].transform;
            targetB = players[1].transform;
        }
        else if (players.Length == 1)
        {
            targetA = players[0].transform;
            targetB = players[0].transform;
        }
    }
}