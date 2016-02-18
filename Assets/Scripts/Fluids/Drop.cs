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
    [System.NonSerialized]
    public DropTarget m_dropTarget;

    [System.NonSerialized]
    public static int s_id = 0;
    [System.NonSerialized]
    public int m_id = 0;

    public bool m_featureHover = false;
    private bool m_goingBack;
    public bool m_featureStop = false;

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
        m_featureHover = true;
        m_featureStop = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!m_underControl)
        {
            m_velocity = -Vector3.up * 10;
        }

        float speedPercent = 1;
        if (m_underControl && m_velocity.magnitude != 0 && m_dropVolume.m_volume != 0)
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

        if (!m_initCollisions.Contains(collider.gameObject) && collider.gameObject.tag != "Drop")
        {
            Drop drop = collider.GetComponent<Drop>();
            //if (!m_waterProjectile || !drop || m_waterProjectile != drop.m_waterProjectile)
            //{
                destroy();
            //}
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (m_initCollisions.Count > 0 && m_initCollisions.Contains(collider.gameObject))
            m_initCollisions.Remove(collider.gameObject);
    }

    public void destroy()
    {
        print("DESTROOOOOOOY");
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
