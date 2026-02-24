using UnityEngine;

public class TetherBreak : MonoBehaviour
{
    public float maxDistance = 10f;
    public float breakForce = 50f;

    private SpringJoint2D joint;
    public TetherVisuals tetherVisuals; // optional: to disable line

    void Start()
    {
        joint = GetComponent<SpringJoint2D>();
        if (joint != null)
            joint.breakForce = breakForce;
    }

    void Update()
    {
        if (joint == null) return;

        // Distance-based snap
        if (joint.connectedBody != null)
        {
            float dist = Vector2.Distance(transform.position, joint.connectedBody.position);
            if (dist > maxDistance)
                BreakTether();
        }
    }

    void OnJointBreak2D(Joint2D brokenJoint)
    {
        BreakTether();
    }

    void BreakTether()
    {
        Destroy(joint);
        joint = null;
        if (tetherVisuals != null)
            tetherVisuals.lineRenderer.enabled = false;
    }
}
