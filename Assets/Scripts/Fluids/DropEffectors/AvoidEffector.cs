using UnityEngine;
using System.Collections;

public class AvoidEffector : MonoBehaviour
{
    private Drop m_drop;
    private Vector3 m_center;
    private float /*m_radius, */m_minSeparationForce, m_maxSeparationForce;
    private float m_splitRatio, m_avoidStrength;

    void Awake()
    {
        m_drop = GetComponent<Drop>();
    }

    void OnEnable()
    {
        m_drop.registerEffector(this);
    }

    public void init(Vector3 _center/*, float _radius*/, float _splitRatio, float _avoidStrength)
    {
        m_center = _center;
        //m_radius = _radius;
        m_splitRatio = _splitRatio;
        m_avoidStrength = _avoidStrength;
    }

    void FixedUpdate()
    {
        Vector3 OM = transform.position - m_center;
        Vector3 z = Vector3.Cross(OM, m_drop.velocity);
        Vector3 n = Vector3.Cross(z, OM);
        Vector3 v1 = Vector3.Project(m_drop.velocity, OM);
        Vector3 v2 = m_drop.velocity - v1;

        if (v2.magnitude == 0 || v1.magnitude > m_splitRatio * v2.magnitude)
            m_drop.split(m_drop.velocity, 4, Mathf.PI / 2 - Mathf.PI / 8);
        else
        {
            float ratio = v1.magnitude / v2.magnitude;
            m_drop.AddForce(n.normalized * m_avoidStrength * ratio * Time.fixedDeltaTime);
        }

        /*float dist = Vector3.Distance(m_center, transform.position) - m_drop.radius;
        if(dist < m_radius)
            m_drop.AddForce((transform.position - m_center).normalized * (m_minSeparationForce + (m_maxSeparationForce - m_minSeparationForce) * (1 - dist/m_radius)) * Time.fixedDeltaTime);
	    */
    }
}
