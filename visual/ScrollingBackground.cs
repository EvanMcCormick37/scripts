using UnityEngine;
using UnityEngine.Serialization;

public class ScrollingBackground : MonoBehaviour
{
    private const string TEX_OFFSET_PROPERTY = "_MainTex";

    [SerializeField, FormerlySerializedAs("_Speed")]
    private float m_speed = 0.1f;

    private Vector2 m_offset = new Vector2(0, 0);
    private void Update()
    {
        float y = Mathf.Repeat(Time.time * m_speed, 1f);
        m_offset.x = 0f;
        m_offset.y = y;
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset(TEX_OFFSET_PROPERTY, m_offset);
    }
}
