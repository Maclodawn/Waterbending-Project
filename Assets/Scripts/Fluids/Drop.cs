using UnityEngine;
using System.Collections;

public class Drop : MonoBehaviour
{
    [System.NonSerialized]
    private Vector3 m_target;
    [System.NonSerialized]
    public Vector3 m_velocity;
    [System.NonSerialized]
    public Vector3 m_gravity;

    [System.NonSerialized]
    public WaterFlow m_waterFlow;

    private float m_volume;

    bool m_goingBack;
    bool m_justSpawned;
    bool m_leftGround;
    bool m_underControl;

    int m_id = 0;
    bool m_collisionTreated = false;

    public const float m_minYPosition = 0;

    public void init(WaterFlow _waterFlow, float _volume, Vector3 _velocity, bool _leftGround, bool _underControl, int _id)
    {
        m_waterFlow = _waterFlow;
        setVolume(_volume);
        m_velocity = _velocity;
        m_underControl = _underControl;
        m_leftGround = _leftGround;
        m_id = _id;
        //gameObject.name += m_id;
        
        m_justSpawned = true;
        m_goingBack = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_collisionTreated = false;

        if (m_underControl)
        {
            // Equation to respect otherwise stretch is needed
            if (m_volume > 2.5f / m_velocity.magnitude)
            {
                stretch(m_volume / 4.0f);
            }

            Vector3 AB = m_target - transform.position;
            float distance = AB.magnitude;
            if (distance >= 0.1f)
            {
                // Going and hovering
                Vector3 velocity = m_gravity * Time.fixedDeltaTime;
                Vector3 v = Vector3.Project(m_velocity, AB.normalized);
                if (v.normalized == AB.normalized)
                {
                    if (m_goingBack)
                        m_velocity += m_velocity;
                    m_velocity += velocity;
                }
                else
                {
                    if (!m_goingBack)
                        m_velocity -= m_velocity;
                    m_velocity -= velocity;
                }
            }
            else
            {
                // Stopping
                m_velocity = Vector3.zero;
            }
        }
        else
        {
            // Going down
            m_velocity += -Vector3.up;
        }

        transform.position += m_velocity * Time.fixedDeltaTime;
    }

    void LateUpdate()
    {
        if (m_justSpawned)
            m_justSpawned = false;

        // In case it misses the terrain collision
        if (transform.position.y < m_minYPosition)
            destroy();
    }

    // Spawn a new drop smaller and reducing the current radius
    private void stretch(float _volume)
    {
        setVolume(m_volume - _volume);

        Drop newDrop;
        if (WaterFlow.s_dropInstancePool.Count == 0)
        {
            newDrop = GameObject.Instantiate<Transform>(m_waterFlow.m_dropPrefab).GetComponent<Drop>();
        }
        else
        {
            newDrop = WaterFlow.s_dropInstancePool[WaterFlow.s_dropInstancePool.Count - 1];
            WaterFlow.s_dropInstancePool.RemoveAt(WaterFlow.s_dropInstancePool.Count - 1);
            newDrop.gameObject.SetActive(true);
        }
        m_waterFlow.m_dropPool.Add(newDrop);

        newDrop.init(m_waterFlow, _volume, m_velocity, m_leftGround, m_underControl, m_id + 1);
        newDrop.transform.position = transform.position - m_velocity.normalized * transform.localScale.x / 2.0f
                                                        + m_velocity.normalized * newDrop.transform.localScale.x / 2.0f;
        if (m_underControl)
        {
            newDrop.updateTarget(m_target);
        }
    }

    void OnTriggerStay(Collider _collider)
    {
        Drop drop = _collider.GetComponent<Drop>();
        if (drop)
        {
            if (m_waterFlow == drop.m_waterFlow)
            {
                // Merge drops
                if (m_collisionTreated || drop.m_collisionTreated)
                {
                    return;
                }

                Drop largestDrop;
                Drop smallestDrop;
                if (transform.localScale.x > drop.transform.localScale.x)
                {
                    largestDrop = this;
                    smallestDrop = drop;
                }
                else
                {
                    smallestDrop = this;
                    largestDrop = drop;
                }

                // Didn't just spawned AND distance between drops less than the largest radius
                if (!smallestDrop.m_justSpawned && Vector3.Distance(largestDrop.transform.position, smallestDrop.transform.position) < largestDrop.transform.localScale.x / 2.0f)
                {
                    largestDrop.setVolume(largestDrop.getVolume() + smallestDrop.getVolume());
                    smallestDrop.destroy();
                }

                // Destroy happen once all triggers are treated, so we need to prevent merge from the smallest one
                m_collisionTreated = true;
                drop.m_collisionTreated = true;
            }
            return;
        }

        TerrainCollider terrain = _collider.GetComponent<TerrainCollider>();
        if (terrain && m_leftGround)
        {
            // Touch the ground
            destroy();
        }
    }

    void OnTriggerExit(Collider _collider)
    {
        TerrainCollider terrain = _collider.GetComponent<TerrainCollider>();
        if (terrain)
        {
            // To prevent a destruction when popping from a reserve
            m_leftGround = true;
        }
    }

    public float getVolume()
    {
        return m_volume;
    }

    // Set volume and update radius
    public void setVolume(float _volume)
    {
        m_volume = _volume;
        float radius = Mathf.Pow((3 * m_volume) / (4 * Mathf.PI), 1.0f / 3.0f);
        transform.localScale = new Vector3(radius, radius, radius);
    }

    // Compute trajectory
    public void updateTarget(Vector3 _target)
    {
        m_target = _target;
        Vector3 AB = m_target - transform.position;
        Vector3 x = AB.normalized;
        Vector3 z = Vector3.Cross(x, new Vector3(0, 1, 0)).normalized;
        Vector3 y = Vector3.Cross(z, x);
        float randomAlpha = Random.value * m_waterFlow.m_deltaAlpha * 2 - m_waterFlow.m_deltaAlpha;
        float randomBeta = Random.value * m_waterFlow.m_deltaBeta * 2 - m_waterFlow.m_deltaBeta;

        m_velocity = (x * Mathf.Cos(m_waterFlow.m_alpha + randomAlpha)
                      + y * Mathf.Sin(m_waterFlow.m_alpha + randomAlpha) * Mathf.Cos(m_waterFlow.m_beta + randomBeta)
                      + z * Mathf.Sin(m_waterFlow.m_alpha + randomAlpha) * Mathf.Sin(m_waterFlow.m_beta + randomBeta))
                    * m_velocity.magnitude;

        Vector3 zv = Vector3.Cross(x, m_velocity).normalized;
        Vector3 yv = Vector3.Cross(zv, x);
        float vx = Vector3.Project(m_velocity, x).magnitude;
        float vy = Vector3.Project(m_velocity, yv).magnitude;

        float g = 2 * vx * vy / AB.magnitude;
        m_gravity = -yv * g;
    }

    // Gravity will be applied if not under control
    public void releaseControl()
    {
        m_underControl = false;
    }

    // Destroy and remove from the waterflow
    public void destroy()
    {
        m_waterFlow.m_dropPool.Remove(this);
        WaterFlow.s_dropInstancePool.Add(this);
        gameObject.SetActive(false);
    }
}
