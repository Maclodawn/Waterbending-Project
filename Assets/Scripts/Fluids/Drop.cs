using UnityEngine;
using System.Collections.Generic;

public class Drop/*Movement*/ : MonoBehaviour
{
    public Vector3 m_gravity;
    [System.NonSerialized]
    public Vector3 m_velocity;
    public float m_initTime = 0.2f;

    private List<GameObject> m_initCollisions = new List<GameObject>();

    [System.NonSerialized]
    public bool m_underControl = true;

    WaterProjectile m_waterProjectile;
    DropVolume m_dropVolume;
    DropTarget m_dropTarget;

    public int m_id = 0;

    private bool m_goingBack;

    void Start()
    {
        m_dropVolume = GetComponent<DropVolume>();
        m_dropTarget = GetComponent<DropTarget>();
    }

    public void init(WaterProjectile _waterProjectile, Vector3 _position, bool _underControl, int _id)
    {
        m_waterProjectile = _waterProjectile;
        transform.position = _position;
        m_underControl = _underControl;
        m_id = _id;
        gameObject.name = m_id.ToString();
    }

    void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_underControl)
        {
            Vector3 AB = m_dropTarget.m_target.transform.position - transform.position;
            float distance = AB.magnitude;
            if (distance >= 0.01f)
            {
                // Going and hovering
                Vector3 velocity = m_gravity * Time.fixedDeltaTime;
                Vector3 v = Vector3.Project(m_velocity, AB.normalized);
                if (v.normalized == AB.normalized)
                {
                    if (m_goingBack)
                    {
                        m_velocity += m_velocity;
                        m_goingBack = false;
                    }
                    m_velocity += velocity;
                }
                else
                {
                    if (!m_goingBack)
                    {
                        m_velocity -= m_velocity;
                        m_goingBack = true;
                    }
                    m_velocity -= velocity;
                }
            }
            else
            {
                m_dropTarget.enabled = false;
                // Stopping
                m_velocity = Vector3.zero;
            }
        }
        else
        {
            m_velocity = -Vector3.up * 10;
        }

        float speedPercent = 1;
        if (m_underControl && m_velocity.magnitude != 0)
        {
            speedPercent = m_dropVolume.m_stretchRatio / (m_dropVolume.m_volume * m_velocity.magnitude);
        }
        transform.position += m_velocity * speedPercent * Time.deltaTime;
    }

    void LateUpdate()
    {
        if (m_initTime > 0)
            m_initTime -= Time.deltaTime;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (m_initTime > 0)
        {
            m_initCollisions.Add(collider.gameObject);
        }

        if (!m_initCollisions.Contains(collider.gameObject))
        {
            Drop drop = collider.GetComponent<Drop>();
            if (!m_waterProjectile || !drop || m_waterProjectile != drop.m_waterProjectile)
            {
                destroy();
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (m_initCollisions.Count > 0 && m_initCollisions.Contains(collider.gameObject))
            m_initCollisions.Remove(collider.gameObject);
    }

    public void destroy()
    {
        if (m_waterProjectile)
            m_waterProjectile.m_dropPool.Remove(this);
        Destroy(gameObject);
    }

//     public void SetTarget(Vector3 _target, Vector3 _velocity)
//     {
// 
//     }
// 
//     public void SetTarget(Vector3 _target)
//     {
//         SetTarget(_target, m_velocity);
//     }
}
