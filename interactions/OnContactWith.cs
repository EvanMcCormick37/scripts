using UnityEngine;
using UnityEngine.Events;

// This allows the event to show up in the Inspector with a dynamic parameter
[System.Serializable]
public class CollisionEvent : UnityEvent<Collider2D> { }

public class OnContactWith : MonoBehaviour
{
    public LayerMask targetLayers;
    public string targetTag;
    public bool loggingEnabled;
    public CollisionEvent onContact;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckTargetMatch(other);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckTargetMatch(collision.collider);
    }

    private void CheckTargetMatch(Collider2D other)
    {
        bool layerMatch = ((1 << other.gameObject.layer) & targetLayers) != 0;
        bool tagMatch = string.IsNullOrEmpty(targetTag) || other.CompareTag(targetTag);

        if (loggingEnabled) print($"{layerMatch} {tagMatch}");

        if (layerMatch && tagMatch)
        {
            onContact.Invoke(other);
        }
    }

    public void Die() => Destroy(gameObject);

    public void DestroyBullet(Collider2D other)
    {
        UbhBullet bullet = GetBulletFromCollider(other);
        UbhObjectPool.instance.ReleaseBullet(bullet);
    }

    public void DestroyEntity(Collider2D entity) => Destroy(entity.gameObject);

    private static UbhBullet GetBulletFromCollider(Collider2D col)
    {
        UbhBullet bullet = col.GetComponent<UbhBullet>();

        bullet ??= col.GetComponentInParent<UbhBullet>();

        return bullet ?? null;
    }
}