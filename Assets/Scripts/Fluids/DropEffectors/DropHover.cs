using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Drop))]
[RequireComponent(typeof(DropTarget))]
public class DropHover : MonoBehaviour
{
    Drop m_drop;
    DropTarget m_dropTarget;

    bool m_goingBack;
    public bool m_stopFeature;
    public bool m_hoverFeature;

    void Start()
    {
        m_drop = GetComponent<Drop>();
        m_dropTarget = GetComponent<DropTarget>();
        m_stopFeature = true;
        m_hoverFeature = true;
    }

    void FixedUpdate()
    {
        Vector3 AB = m_dropTarget.m_target.transform.position - transform.position;
        float distance = AB.magnitude;
        if (distance >= 0.01f || !m_stopFeature)
        {
            // Hovering
            if (m_hoverFeature)
            {
                Vector3 v = Vector3.Project(m_drop.velocity, AB.normalized);
                if (v.normalized == AB.normalized)
                {
                    if (m_goingBack)
                    {
                        m_drop.AddForce(-m_drop.velocity * 7.0f / 6.0f);
                        m_goingBack = false;
                    }
                }
                else
                {
                    if (!m_goingBack)
                    {
                        m_drop.AddForce(-m_drop.velocity * 7.0f / 6.0f);
                        m_goingBack = true;
                    }
                }
            }
        }
        else if (m_dropTarget.enabled)
        {
            // Stopping
            m_dropTarget.enabled = false;
            m_drop.AddForce(-m_drop.velocity);
        }
    }
}
