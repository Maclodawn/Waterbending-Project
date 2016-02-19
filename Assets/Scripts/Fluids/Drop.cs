using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(DropVolume))]
public class Drop/*Movement*/ : MonoBehaviour
{
    private Vector3 m_velocity;
    public float m_initTime = 0.2f;

    private List<GameObject> m_initCollisions = new List<GameObject>();

    private DropVolume m_dropVolume;
    [System.NonSerialized]
    public DropTarget m_dropTarget;

    public WaterGroup m_waterGroup;

    private List<MonoBehaviour> m_effectors = new List<MonoBehaviour>();

    public Vector3 velocity
    {
        get
        {
            return m_velocity;
        }
    }

    void Start()
    {
        m_dropVolume = GetComponent<DropVolume>();
    }

    public void registerEffector(MonoBehaviour _effector)
    {
        m_effectors.Add(_effector);
    }

    public void removeEffectors()
    {
        foreach(MonoBehaviour effector in m_effectors)
            Destroy(effector);
        m_effectors.Clear();
    }

    // Used ONLY for initialization, otherwise use AddForce
    public void init(Vector3 _position, WaterGroup _waterGroup)
    {
        transform.position = _position;
        m_waterGroup = _waterGroup;
    }

    // Used ONLY for initialization, otherwise use AddForce
    public void initVelocity(Vector3 _velocity)
    {
        m_velocity = _velocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float speedPercent = 1;
        if (m_velocity.magnitude != 0 && m_dropVolume.m_volume != 0 && m_dropTarget)
        {
            speedPercent = m_dropVolume.m_stretchRatio / (m_dropVolume.m_volume * m_dropTarget.m_initialVelocity);
        }
        transform.position += m_velocity * speedPercent * Time.fixedDeltaTime;
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

        if (!m_initCollisions.Contains(collider.gameObject) && collider.GetComponent<Drop>() == null && collider.GetComponent<WaterDetector>() == null)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (m_initCollisions.Count > 0 && m_initCollisions.Contains(collider.gameObject))
            m_initCollisions.Remove(collider.gameObject);
    }

    void OnDestroy()
    {
        if (m_waterGroup)
            m_waterGroup.m_dropPool.Remove(this);
    }

    public void AddForce(Vector3 _force)
    {
        m_velocity += _force;
    }

    //TODO DestroyAllDropEffectors

    public void releaseControl()
    {
        Destroy(GetComponent<DropHover>());
        Destroy(m_dropTarget);
        gameObject.AddComponent<DropGravity>();
    }
}
