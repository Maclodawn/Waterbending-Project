using UnityEngine;
using System.Collections;

public class RotateEffector : MonoBehaviour
{
    Vector3 m_center;
    Vector3 m_normal;
    float m_pullForce;
    float m_friction = 0.5f;
    
	void Start ()
    {
        m_friction = 10f;
	}

    public void init(Vector3 _center, Vector3 _normal, float _pullForce)
    {
        Drop drop = GetComponent<Drop>();
        m_center = _center;
        m_normal = _normal;
        m_pullForce = _pullForce;
        Vector3 OM = Vector3.ProjectOnPlane(transform.position - m_center, Vector3.up);
        Vector3 x = -OM.normalized;
        Vector3 z = Vector3.Cross(Vector3.ProjectOnPlane(drop.velocity, Vector3.up), x);
        Vector3 y = Vector3.Cross(z, x);
        drop.initVelocity(y * drop.velocity.magnitude);
    }
	
	void FixedUpdate ()
    {
        Drop drop = GetComponent<Drop>();
        Vector3 OM = Vector3.ProjectOnPlane(transform.position - m_center, Vector3.up);

        Vector3 x = -OM.normalized;
        Vector3 z = Vector3.Cross(drop.velocity, x).normalized;
        Vector3 y = Vector3.Cross(z, x);

        drop.initVelocity(-y * drop.velocity.magnitude);

        drop.AddForce((x * drop.velocity.magnitude) * Time.fixedDeltaTime);
	}
}
