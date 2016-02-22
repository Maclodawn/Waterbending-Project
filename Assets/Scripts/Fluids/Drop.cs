using UnityEngine;
using System.Collections.Generic;

public class Drop/*Movement*/ : MonoBehaviour
{
    public Vector3 m_velocity;
    public float m_initTime = 0.2f;

    private List<GameObject> m_initCollisions = new List<GameObject>();

    public WaterGroup m_waterGroup;

    private DropVolume m_dropVolume;

    private List<MonoBehaviour> m_effectors = new List<MonoBehaviour>();

    public float radius { get { return transform.localScale.x/2; } }

    public Transform m_dropPrefab;

    public Vector3 velocity
    {
        get
        {
            return m_velocity;
        }
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
        if (getDropVolume() && m_velocity.magnitude != 0 && getDropVolume().m_volume != 0 && !GetComponent<DropGravity>())
        {
            speedPercent = getDropVolume().m_stretchRatio / (getDropVolume().m_volume * getDropVolume().m_initialSpeed);
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

    private DropVolume getDropVolume()
    {
        if (m_dropVolume)
            return m_dropVolume;

        m_dropVolume = GetComponent<DropVolume>();
        return m_dropVolume;
    }

    private bool test = false;
    public void split(Vector3 splitDirection, int count, float alpha)
    {
        Vector3 x, y, z = m_velocity.normalized;
        if (Vector3.Dot(z, Vector3.right) == m_velocity.magnitude)
            x = Vector3.Cross(z, Vector3.right);
        else x = Vector3.Cross(z, Vector3.up);
        
        y = Vector3.Cross(z, x);

        float initAngle = Random.value * 2 * Mathf.PI;
        /*for (int i = 0; i < count; i++)
        {
            //Drop drop = instance.GetComponent<Drop>();
            float beta = initAngle;
            Vector3 speedDir = x * Mathf.Sin(alpha) * Mathf.Cos(beta) + y * Mathf.Sin(alpha) * Mathf.Sin(beta) + z * Mathf.Cos(alpha);
            Transform newDrop = Instantiate<Transform>(m_dropPrefab);
            newDrop.position = transform.position;
            Drop drop = newDrop.GetComponent<Drop>();
            drop.initVelocity(speedDir * m_velocity.magnitude);
            drop.gameObject.SetActive(false);
            print(speedDir);
            drop.gameObject.SetActive(true);
        }*/
        //Destroy(gameObject);loat beta = initAngle;
        float beta = initAngle;
        Vector3 speedDir = x * Mathf.Sin(alpha) * Mathf.Cos(beta) + y * Mathf.Sin(alpha) * Mathf.Sin(beta) + z * Mathf.Cos(alpha);
        m_velocity = speedDir * velocity.magnitude; 
    }
}
