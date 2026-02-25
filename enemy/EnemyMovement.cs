using UnityEngine;
using UnityEngine.Serialization;

public class EnemyMovement : UbhMonoBehaviour
{
    public Transform target;
    public bool loggingEnabled;
    public float obstacleDetectionDistance = 15f;
    public float minObstacleDistance = 0.2f;
    public float speed = 0.5f;
    public float maxSpeed = 10f;
    public LayerMask environmentLayer;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            FindTarget();
        }
        else
        {
            Move();
        }
    }
    public void Move()
    {
        Vector2 direction = GetTargetDirection();
        Vector2 avoidanceDirection = GetCombinedAvoidance(direction);
        if (avoidanceDirection != Vector2.zero)
        {
            if (loggingEnabled) Debug.Log($"Direction pre-norm: {direction}");
            if (loggingEnabled) Debug.Log($"Avoidance direction pre-norm: {avoidanceDirection}");
            direction = Vector2.Lerp(direction, avoidanceDirection, 0.8f);
            if (loggingEnabled) Debug.Log($"Direction post-norm: {direction}");
        }
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
        rb.linearVelocity = Vector2.ClampMagnitude(direction * speed, maxSpeed);
    }

    private Vector2 GetTargetDirection()
    {
        Vector2 targetPos = new Vector2(target.position.x, target.position.y);
        Vector2 currentPos = gameObject.transform.position;
        Vector2 direction = targetPos - currentPos;
        return direction;
    }
    private Vector2 GetCombinedAvoidance(Vector2 dirToPlayer)
    {
        Collider2D[] obstacles = Physics2D.OverlapCircleAll(transform.position, obstacleDetectionDistance, environmentLayer);

        if (obstacles.Length == 0)
        {
            if (loggingEnabled) Debug.Log("No obstacles.");
            return dirToPlayer;
        }

        Vector2 avoidanceForce = Vector2.zero;

        foreach (var obstacle in obstacles)
        {
            Vector2 closestPoint = obstacle.ClosestPoint(transform.position);
            Vector2 awayFromObstacle = (Vector2)transform.position - closestPoint;

            float distance = awayFromObstacle.magnitude;
            avoidanceForce += awayFromObstacle.normalized / Mathf.Max(distance - minObstacleDistance, 0.001f);
        }

        Vector2 tangent = new Vector2(avoidanceForce.y, -avoidanceForce.x);

        return (tangent + avoidanceForce) * 2f;
    }
    private void FindTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("K");
        if (player != null)
        {
            target = player.transform;
        }
    }
}