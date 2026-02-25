using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float accelerationForce = 50f;
    public float maxSpeed = 2f;
    public bool movement_disabled = false;
    public bool accelerate = true;
    public int fuel = 100;
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
        var keyboard = Keyboard.current;

        if (accelerate) { rb.AddForce(moveInput.normalized * accelerationForce); }
        else { rb.linearVelocity = moveInput.normalized * maxSpeed; }

        if (rb.linearVelocity.magnitude > maxSpeed) rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
        if (keyboard.spaceKey.isPressed && fuel > 0) { rb.linearVelocity *= 2; BurnFuel(); }

    }

    private void BurnFuel()
    {
        fuel -= 1;
    }
}
