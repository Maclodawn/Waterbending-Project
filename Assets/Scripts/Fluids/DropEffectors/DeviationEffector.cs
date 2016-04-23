using UnityEngine;
using System.Collections;

public class DeviationEffector : MonoBehaviour
{
    private Drop m_drop;
    [System.NonSerialized]
    public GameObject m_target;
    public float m_targetRadius { get; private set; }
    //FIXME
    private float m_k = 0;
    //FIXME
    private float m_T = 0;
    //FIXME
    private float m_time = 0;
    private Vector3 m_forceDir;
    public bool destinationReached { get { return m_time > 2 * m_T; } }
    private Vector3 m_destination;
    //FIXME
    private float m_fy;
    //FIXME
    private float m_l;

    private bool m_willTurnAround = true;

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
        init(_target, _targetRadius, true);
    }

    public void init(GameObject _target, float _targetRadius, bool _willTurnAround)
    {
        m_target = _target;
        m_targetRadius = _targetRadius;
        m_willTurnAround = _willTurnAround;

        Vector3 targetPosition = m_target.transform.position;
        CharacterController targetController = m_target.GetComponent<CharacterController>();
        if (targetController)
        {
            targetPosition += targetController.center;
        }

        if (m_drop.velocity == Vector3.zero)
        {
            m_drop.AddForce((targetPosition - transform.position).normalized * m_drop.m_initialSpeed);
        }
        
        //FIXME Why vy couldn't be null?
        if (m_drop.velocity.y == 0)
        {
            m_drop.initVelocity(m_drop.velocity + 0.01f * Vector3.up);
        }

        Vector3 velocityNormalizedWithoutY = m_drop.velocity;
        velocityNormalizedWithoutY.y = 0;
        velocityNormalizedWithoutY.Normalize();

        float vy = m_drop.velocity.y;

        Vector3 vectPosToTargetWithoutY = targetPosition - transform.position;
        float dy = vectPosToTargetWithoutY.y;
        vectPosToTargetWithoutY.y = 0;

        Vector3 z = Vector3.Cross(velocityNormalizedWithoutY, vectPosToTargetWithoutY).normalized;
        if (z == Vector3.zero)
            z = -Vector3.up;
        Vector3 x = Vector3.Cross(velocityNormalizedWithoutY, z);
        //Why divided by 2? -> To do a sinusoid
        Vector3 ON = (vectPosToTargetWithoutY + x * m_targetRadius) / 2;
        float ONx = Vector3.Project(ON, x).magnitude;
        float ONy = Vector3.Project(ON, velocityNormalizedWithoutY).magnitude;

        Vector3 velocityWithoutY = m_drop.velocity;
        velocityWithoutY.y = 0;

        m_T = ONy / velocityWithoutY.magnitude;
        m_k = 2 * velocityWithoutY.sqrMagnitude * ONx / ONy / ONy;
        m_forceDir = x;

        //m_fy = dy / m_T / m_T - 3 / 2 * vy / m_T;

        //m_l = 1 + vy / m_T / m_fy;

        m_l = (2 * dy - vy * m_T) / (2 * dy - 3 * vy * m_T);
        m_fy = vy / m_T / (m_l - 1);

        if (float.IsNaN(m_fy))
        {
            Debug.Break();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_time < 2 * m_T)
        {
            m_drop.AddForce((m_forceDir * m_k * (m_time > m_T ? -1 : 1) + (m_time > m_T ? -m_l : 1) * m_fy * Vector3.up) * Time.fixedDeltaTime);
            m_drop.initVelocity(m_drop.velocity.normalized * m_drop.m_initialSpeed);
        }
        else if (m_willTurnAround)
        {
            m_drop.removeEffectors();
            m_drop.gameObject.AddComponent<RotateEffector>();
            m_drop.GetComponent<RotateEffector>().init(m_target, Vector3.up/*, 1*/, m_targetRadius);
        }
        m_time += Time.fixedDeltaTime;
    }
}
