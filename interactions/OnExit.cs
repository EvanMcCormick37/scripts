using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;

public class OnExit : MonoBehaviour
{
    public LayerMask targetLayers;
    public string targetTag;
    public bool loggingEnabled;
    public UnityEvent<Collider2D> onExit;

    private void OnTriggerExit2D(Collider2D c)
    {
        if (CheckTargetMatch(c))
        {
            onExit.Invoke(c);
        }
    }
    private bool CheckTargetMatch(Collider2D other)
    {
        bool layerMatch = ((1 << other.gameObject.layer) & targetLayers) != 0;
        bool tagMatch = string.IsNullOrEmpty(targetTag) || other.CompareTag(targetTag);

        if (loggingEnabled) print($"{layerMatch} {tagMatch}");

        return layerMatch && tagMatch;
    }

    public void Die() => Destroy(gameObject);

    public void DestroyBullet(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            UbhBullet bullet = GetBulletFromCollider(other);
            UbhObjectPool.instance.ReleaseBullet(bullet);
        }
    }

    public void DestroyEntity(Collider2D entity) => Destroy(entity.gameObject);

    private static UbhBullet GetBulletFromCollider(Collider2D col)
    {
        UbhBullet bullet = col.GetComponent<UbhBullet>();

        bullet ??= col.GetComponentInParent<UbhBullet>();

        return bullet ?? null;
    }
}