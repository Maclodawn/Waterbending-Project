using UnityEngine;
using System.Collections;

public class DropHover : MonoBehaviour
{
    Drop m_drop;
    DropTarget m_dropTarget;

    bool m_goingBack;
    public bool m_stopFeature;
    public bool m_hoverFeature;

    void Start()
    {
        m_dropTarget = GetComponent<DropTarget>();
        m_stopFeature = true;
        m_hoverFeature = true;
    }

    void FixedUpdate()
    {
        Vector3 AB = m_dropTarget.m_target.transform.position - transform.position;
        float distance = AB.magnitude;
        if (distance >= 0.01f)
        {
            // Going and hovering
            Vector3 velocity = m_drop.m_gravity * Time.fixedDeltaTime;
            Vector3 v = Vector3.Project(m_drop.velocity, AB.normalized);
            if (v.normalized == AB.normalized)
            {
                if (m_goingBack)
                {
                    m_drop.AddForce(-m_drop.velocity * 7.0f / 6.0f);
                    m_goingBack = false;
                }
                m_drop.AddForce(velocity);
            }
            else
            {
                if (!m_goingBack)
                {
                    m_drop.AddForce(-m_drop.velocity * 7.0f / 6.0f);
                    m_goingBack = true;
                }
                m_drop.AddForce(-velocity);
            }
        }
        else
        {
            m_dropTarget.enabled = false;
            // Stopping
            m_drop.AddForce(-m_drop.velocity);
        }
    }
}
