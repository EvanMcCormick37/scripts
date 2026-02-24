using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float accelerationForce = 50f;
    public float maxSpeed = 2f;
    public bool movement_disabled = false;
    public bool accelerate = true;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (movement_disabled) return;
        var keyboard = Keyboard.current;
        moveInput = Vector2.zero;
        if (keyboard.upArrowKey.isPressed || keyboard.wKey.isPressed) moveInput.y += 1;
        if (keyboard.downArrowKey.isPressed || keyboard.sKey.isPressed) moveInput.y -= 1;
        if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed) moveInput.x += 1;
        if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed) moveInput.x -= 1;
        moveInput = moveInput.normalized;
    }

    void FixedUpdate()
    {

        if (accelerate)
        {
            rb.AddForce(moveInput.normalized * accelerationForce);
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }
        else
        {
            rb.linearVelocity = moveInput.normalized * maxSpeed;
        }
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }
    }
}
