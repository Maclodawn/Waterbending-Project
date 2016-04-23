using UnityEngine;
using System.Collections;

public class RotateEffector : MonoBehaviour
{
    [System.NonSerialized]
    public GameObject m_target;
    Vector3 m_center;
//     Vector3 m_normal;
//     float m_pullForce;
//     float m_friction = 0.5f;
    private Drop m_drop;
    public float m_radiusToTurnAround { get; private set; }

    void Awake()
    {
        m_drop = GetComponent<Drop>();
    }

    void OnEnable()
    {
        m_drop.registerRotate(this);
    }

//     void Start()
//     {
//         m_friction = 10f;
//     }

    public void init(Vector3 _center, Vector3 _normal/*, float _pullForce*/)
    {
        m_center = _center;
//         m_normal = _normal;
//        m_pullForce = _pullForce;
        Vector3 OM = Vector3.ProjectOnPlane(transform.position - m_center, Vector3.up);
        Vector3 x = -OM.normalized;
        Vector3 z = Vector3.Cross(Vector3.ProjectOnPlane(m_drop.velocity, Vector3.up), x);
        Vector3 y = Vector3.Cross(z, x);
        m_drop.initVelocity(-y * m_drop.velocity.magnitude);
    }

    public void init(GameObject _target, Vector3 _normal/*, float _pullForce*/, float _radiusToTurnAround)
    {
        m_target = _target;
        m_center = m_target.transform.position;
        CharacterController targetController = m_target.GetComponent<CharacterController>();
        if (targetController)
        {
            m_center += targetController.center;
        }
//        m_normal = _normal;
//        m_pullForce = _pullForce;
        m_radiusToTurnAround = _radiusToTurnAround;
        Vector3 OM = Vector3.ProjectOnPlane(transform.position - m_center, Vector3.up);
        Vector3 x = -OM.normalized;
        Vector3 z = Vector3.Cross(Vector3.ProjectOnPlane(m_drop.velocity, Vector3.up), x).normalized;
        Vector3 y = Vector3.Cross(z, x);
        m_drop.initVelocity(-y * m_drop.velocity.magnitude);
    }

    void FixedUpdate()
    {
        Vector3 OM = Vector3.ProjectOnPlane(transform.position - m_center, Vector3.up);

        Vector3 x = -OM.normalized;
        Vector3 z = Vector3.Cross(m_drop.velocity, x).normalized;
        Vector3 y = Vector3.Cross(z, x);

//        m_drop.initVelocity(-y * m_drop.velocity.magnitude);
        Vector3 futureVelocity = -y * m_drop.velocity.magnitude;

//        m_drop.AddForce((x * m_drop.velocity.magnitude) * Time.fixedDeltaTime);

        // Correction of error offset
        Vector3 futurePos = m_drop.transform.position + futureVelocity * Time.fixedDeltaTime;

        Vector3 futureOM = Vector3.ProjectOnPlane(futurePos - m_center, Vector3.up);

        Vector3 futureX = -futureOM.normalized;
        //Vector3 futureZ = Vector3.Cross(futureVelocity, futureX).normalized;
        //Vector3 futureY = Vector3.Cross(futureZ, futureX);

        Vector3 error = (Vector3.Distance(m_center, futurePos) - m_radiusToTurnAround) * futureX;

        Vector3 futurePosWanted = futurePos + error;
        Vector3 dirWanted = (futurePosWanted - transform.position).normalized;
        m_drop.initVelocity(dirWanted * futureVelocity.magnitude);
    }
}
