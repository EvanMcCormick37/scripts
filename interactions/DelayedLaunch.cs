using UnityEngine;
using System.Collections;

public class DelayedLaunch : MonoBehaviour
{
    public float launchDelay = 3f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isCharging = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // This is the function you will link in the OnContact Inspector
    public void StartLaunchTimer(Collider2D other)
    {
        // Prevent overlapping timers if hit multiple times
        if (isCharging) return;

        // 1. Capture the momentum from the "other" object (the Player)
        Rigidbody2D playerRb = other.attachedRigidbody;
        if (playerRb != null)
        {
            Vector2 capturedVelocity = playerRb.linearVelocity;
            StartCoroutine(LaunchSequence(capturedVelocity));
        }
    }

    private IEnumerator LaunchSequence(Vector2 momentum)
    {
        isCharging = true;

        // 2. Visual Feedback: Turn Red
        Color originalColor = sr.color;
        sr.color = Color.red;

        // 3. The 5-second timer
        yield return new WaitForSeconds(launchDelay);

        // 4. Impart the momentum
        // Ensure the Rigidbody isn't Static or it won't move
        if (rb.bodyType == RigidbodyType2D.Static)
            rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = momentum;

        // Optional: Reset color or destroy after launch
        sr.color = originalColor;
        isCharging = false;
    }
}