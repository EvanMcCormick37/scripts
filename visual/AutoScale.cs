using UnityEngine;
using UnityEngine.Serialization;


public class AutoScale : MonoBehaviour
{
    [SerializeField]
    private float m_scale = 2f;
    private void Start()
    {
        GameManager manager = FindObjectOfType<GameManager>();
        if (manager != null && manager.m_scaleToFit)
        {
            Vector2 max = Camera.main.ViewportToWorldPoint(UbhUtil.VECTOR2_ONE);
            Vector2 scale = max * m_scale;
            transform.localScale = scale;
        }
    }

}