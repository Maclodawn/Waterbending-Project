using UnityEngine;
using System.Collections;

public class DropVolume : MonoBehaviour
{

    Drop m_dropMovement;
    DropTarget m_dropTarget;
    WaterProjectile m_waterProjectile;

    public float m_volume { get; private set; }

    public float m_stretchRatio { get; private set; }

    public void setVolume(float _volume)
    {
        m_volume = _volume;
        float radius = Mathf.Pow((3 * m_volume) / (4 * Mathf.PI), 1.0f / 3.0f);
        transform.localScale = new Vector3(radius, radius, radius);
    }

    // Use this for initialization
    void Start()
    {
        m_dropMovement = GetComponent<Drop>();
        m_dropTarget = GetComponent<DropTarget>();
        m_stretchRatio = 2.5f;
    }

    public void init(WaterProjectile _waterProjectile, float _volume)
    {
        m_waterProjectile = _waterProjectile;
        setVolume(_volume);
    }

    // Update is called once per frame
    void Update()
    {
        // Equation to respect otherwise stretch is needed
        float newVolume = m_stretchRatio / m_dropMovement.m_velocity.magnitude;
        if (m_volume > newVolume)
        {
            stretch(m_volume - newVolume);
        }
    }

    // Spawn a new drop smaller and reducing the current radius
    private void stretch(float _volume)
    {
        //Debug.Break();
        setVolume(m_volume - _volume);

        Drop newDrop = GameObject.Instantiate<Transform>(m_waterProjectile.m_dropPrefab).GetComponent<Drop>();
        m_waterProjectile.m_dropPool.Add(newDrop);

        Vector3 position = transform.position /* - m_velocity.normalized * transform.localScale.x / 2.0f
                                                + m_velocity.normalized * newDrop.transform.localScale.x / 2.0f*/;
        newDrop.init(m_waterProjectile, position, m_dropMovement.m_underControl, m_dropMovement.m_id + 1);
        newDrop.m_velocity = m_dropMovement.m_velocity;

        DropTarget newDropTarget = newDrop.GetComponent<DropTarget>();
        newDropTarget.enabled = m_dropMovement.m_underControl;
        if (m_dropMovement.m_underControl)
        {
            newDropTarget.Init(m_dropTarget.m_target, newDrop.m_velocity);
        }

        newDrop.GetComponent<DropVolume>().init(m_waterProjectile, _volume);
    }

    void OnTriggerStay(Collider _collider)
    {
        Drop drop = _collider.GetComponent<Drop>();
        if (drop)
        {
//             if (m_waterProjectile == drop.m_waterProjectile)
//             {
//                 // Merge drops
//                 if (m_collisionTreated || drop.m_collisionTreated)
//                 {
//                     return;
//                 }
// 
//                 Drop largestDrop;
//                 Drop smallestDrop;
//                 if (transform.localScale.x > drop.transform.localScale.x)
//                 {
//                     largestDrop = this;
//                     smallestDrop = drop;
//                 }
//                 else
//                 {
//                     smallestDrop = this;
//                     largestDrop = drop;
//                 }
// 
//                 // Didn't just spawned AND distance between drops less than the largest radius
//                 if (!smallestDrop.m_justSpawned && Vector3.Distance(largestDrop.transform.position, smallestDrop.transform.position) < largestDrop.transform.localScale.x / 2.0f)
//                 {
//                     largestDrop.setVolume(largestDrop.getVolume() + smallestDrop.getVolume());
//                     Destroy(smallestDrop.gameObject);
//                 }
// 
//                 // Destroy happen once all triggers are treated, so we need to prevent merge from the smallest one
//                 m_collisionTreated = true;
//                 drop.m_collisionTreated = true;
//             }
            return;
        }
    }
}
