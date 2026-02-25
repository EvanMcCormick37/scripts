using UnityEngine;
using UnityEngine.InputSystem;

public class CursorMovement : UbhMonoBehaviour
{
    [Header("Movement Settings")]
    public float followStrength = 0.1f;
    public float maxSpeed = 50f;

    [Header("Fuel Settings")]
    public float maxFuel = 50f;
    public float fuelConsumptionRate = 20f;
    public float currentFuel;

    [Header("Visuals")]
    public Color fullColor = Color.red;
    public Color emptyColor = Color.black;
    private SpriteRenderer spriteRenderer;
    public bool loggingEnabled;

    private Rigidbody2D rb;
    private Camera mainCam;
    public bool disabled = false;

    void Start()
    {
        mainCam = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;

        currentFuel = maxFuel;
    }

    void FixedUpdate()
    {
        if (disabled) return;

        bool isClicking = Mouse.current.leftButton.isPressed;
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.spaceKey.isPressed && currentFuel > 0 && rb.linearVelocity.magnitude < maxSpeed)
        {
            if (loggingEnabled) Debug.Log($"C Speed: {rb.linearVelocity}");
            Swing();
            ConsumeFuel();
        }

        if (isClicking && currentFuel > 0 && rb.linearVelocity.magnitude < maxSpeed)
        {
            MoveTowardsMouse();
            ConsumeFuel();
        }
    }

    private void UpdateVisuals()
    {
        if (spriteRenderer == null) return;
        float fuelPercent = currentFuel / maxFuel;
        spriteRenderer.color = Color.Lerp(emptyColor, fullColor, fuelPercent);
    }

    private void MoveTowardsMouse()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        Vector3 targetWorldPos = mainCam.ScreenToWorldPoint(new Vector3(
            mouseScreenPos.x,
            mouseScreenPos.y,
            Mathf.Abs(mainCam.transform.position.z)
        ));

        targetWorldPos.z = 0f;

        Vector2 currentPos = rb.position;
        Vector2 directionToMouse = (Vector2)targetWorldPos - currentPos;

        Vector2 clampedDirection = Vector2.ClampMagnitude(directionToMouse, maxSpeed);

        rb.linearVelocity = clampedDirection * followStrength * 10;
    }

    private void Swing()
    {
        Vector2 currentPos = rb.position;
        GameObject partner = GameObject.FindGameObjectWithTag("K");
        Vector2 partnerPos = partner.transform.position;
        Vector2 diff = currentPos - partnerPos;
        Vector2 direction = new Vector2(
            -diff.y,
            diff.x
        );
        rb.AddForce(direction * followStrength);
    }


    private void ConsumeFuel()
    {
        currentFuel -= fuelConsumptionRate * Time.fixedDeltaTime;
        currentFuel = Mathf.Max(currentFuel, 0);
        UpdateVisuals();

        if (loggingEnabled && currentFuel <= 0) Debug.Log("Out of fuel!");
    }

    public void AddFuel()
    {
        currentFuel = Mathf.Min(currentFuel + 1, maxFuel);
        UpdateVisuals();
    }
}