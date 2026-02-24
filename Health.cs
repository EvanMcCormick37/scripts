using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int health = 10;
    public UnityEvent onDeath;

    public void TakeDamage()
    {
        health -= 1;
        if (health <= 0) onDeath.Invoke();
    }
}