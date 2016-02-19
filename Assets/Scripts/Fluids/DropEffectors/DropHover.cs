using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Drop))]
public class DropHover : MonoBehaviour
{
    Drop m_drop;
    [System.NonSerialized]
    public GameObject m_target;

    bool m_goingBack;
    public bool m_stopFeature;
    public bool m_hoverFeature;

    void Start()
    {
        m_drop = GetComponent<Drop>();
    }

    public void init(GameObject _target, bool _stopFeature, bool _hoverFeature)
    {
        m_target = _target;
        m_stopFeature = _stopFeature;
        m_hoverFeature = _hoverFeature;
    }

    void FixedUpdate()
    {
        Vector3 AB = m_target.transform.position - transform.position;
        float distance = AB.magnitude;
        if (distance >= transform.localScale.x / 4.0f || !m_stopFeature)
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
        else
        {
            // Stopping
            m_drop.AddForce(-m_drop.velocity);
        }
    }
}
