using UnityEngine;
using System.Collections;

public class AvoidEffector : MonoBehaviour {
    private Drop m_drop;
    private Vector3 m_center;
    private float m_radius, m_minSeparationForce, m_maxSeparationForce;

    void Awake()
    {
        m_drop = GetComponent<Drop>();
    }

    void OnEnable()
    {
        m_drop.registerEffector(this);
    }

    public void init(Vector3 _center, float _radius, float _minSeparationForce, float _maxSeparationForce)
    {
        m_center = _center;
        m_radius = _radius;
        m_minSeparationForce = _minSeparationForce;
        m_maxSeparationForce = _maxSeparationForce;
    }

    void FixedUpdate()
    {
        float dist = Vector3.Distance(m_center, transform.position) - m_drop.radius;
        if(dist < m_radius)
            m_drop.AddForce((transform.position - m_center).normalized * (m_minSeparationForce + (m_maxSeparationForce - m_minSeparationForce) * (1 - dist/m_radius)) * Time.fixedDeltaTime);
	}
}
