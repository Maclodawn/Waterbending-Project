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
    private float m_fy, m_l;

    void OnEnable()
    {
        m_drop.registerEffector(this);
    }

    void Awake()
    {
        m_drop = GetComponent<Drop>();
    }

    public void init(GameObject _target, float _targetRadius)
    {
        m_target = _target;
        m_targetRadius = _targetRadius;
        Vector3 y = m_drop.velocity;
        float vy = m_drop.velocity.y;
        y.y = 0;
        y.Normalize();
        Vector3 OC = _target.transform.position - transform.position;
        float dy = OC.y;
        OC.y = 0;
        Vector3 z = Vector3.Cross(y, OC).normalized;
        Vector3 x = Vector3.Cross(y, z);
        Vector3 ON = (OC + x * m_targetRadius) / 2;
        float ONx = Vector3.Project(ON, x).magnitude;
        float ONy = Vector3.Project(ON, y).magnitude;
        Vector3 vxy = m_drop.velocity;
        vxy.y = 0;
        m_T = ONy / vxy.magnitude;
        m_k = 2 * vxy.sqrMagnitude * ONx / ONy / ONy;
        m_forceDir = x;

        //m_fy = dy / m_T / m_T - 3 / 2 * vy / m_T;

        //m_l = 1 + vy / m_T / m_fy;

        m_l = (2 * dy - vy * m_T) / (2 * dy - 3 * vy * m_T);
        m_fy = vy / m_T / (m_l - 1);
        
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (m_time < 2 * m_T)
        {
            m_drop.AddForce((m_forceDir * m_k * (m_time > m_T ? -1 : 1) + (m_time > m_T ? -m_l : 1) * m_fy * Vector3.up) * Time.fixedDeltaTime);
        }
        else {
            m_drop.removeEffectors();
            m_drop.gameObject.AddComponent<RotateEffector>();
            m_drop.GetComponent<RotateEffector>().init(m_target.transform.position, Vector3.up, 1);
        }
        m_time += Time.fixedDeltaTime;
    }
}
