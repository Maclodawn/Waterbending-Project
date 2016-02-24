using UnityEngine;
using System.Collections;

public class DeviationEffector : MonoBehaviour {
    private Drop m_drop;
    private GameObject m_target;
    private float m_targetRadius;
    private float m_k, m_T, m_time = 0;
    private Vector3 m_forceDir;
    public bool destinationReached { get { return m_time > 2 * m_T; } }
    private Vector3 m_destination;

    void Awake()
    {
        m_drop = GetComponent<Drop>();
    }

    public void init(GameObject _target, float _targetRadius)
    {
        m_target = _target;
        m_targetRadius = _targetRadius;
        Vector3 OC = _target.transform.position - transform.position;
        Vector3 y = m_drop.velocity.normalized;
        Vector3 z = Vector3.Cross(OC, y).normalized;
        Vector3 x = Vector3.Cross(y, z).normalized;

        Vector3 targetPos = _target.transform.position;
        Vector3 OM = OC - x * _targetRadius;
        m_destination = targetPos;
        float Mx = Vector3.Project(OM, x).magnitude, My = Vector3.Project(OM, y).magnitude;
        m_T = My / m_drop.velocity.magnitude / 2;
        m_k = Mx / m_T / m_T;
        m_forceDir = x;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (m_time < 2 * m_T)
        {
            m_drop.AddForce(m_forceDir * m_k * (m_time > m_T ? -1 : 1) * Time.fixedDeltaTime);
        }
        else {
            m_drop.initVelocity(new Vector3(0, 0, 0));
            print(Vector3.Distance(m_destination, transform.position));
            Destroy(this);
        }
        m_time += Time.fixedDeltaTime;
    }
}
