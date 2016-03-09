using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Drop))]
public class DropPullEffector : MonoBehaviour
{
    Drop m_drop;
    [System.NonSerialized]
    public GameObject m_target;
    private float m_initialSpeed;

    void Awake()
    {
        m_drop = GetComponent<Drop>();
    }

    public void init(GameObject _target, float _initialSpeed)
    {
        m_target = _target;
        m_initialSpeed = _initialSpeed;
    }

    void OnEnable()
    {
        m_drop.registerEffector(this);
    }

    void FixedUpdate()
    {
        Vector3 AB = m_target.transform.position - transform.position;
        float distance = AB.magnitude;
        if (distance >= transform.localScale.x / 4.0f)
        {
            m_drop.AddForce(-m_drop.velocity);
            m_drop.AddForce((m_target.transform.position - transform.position).normalized * m_initialSpeed);
        }
        else if (m_drop.velocity != Vector3.zero)
        {
            // Stopping
            m_drop.AddForce(-m_drop.velocity);
        }
    }

}
