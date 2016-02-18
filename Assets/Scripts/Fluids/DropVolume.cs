using UnityEngine;
using System.Collections;

public class DropVolume : MonoBehaviour
{

    Drop m_dropMovement;
    DropTarget m_dropTarget;
    WaterProjectile m_waterProjectile;

    public float m_volume { get; private set; }

    public float m_stretchRatio { get; private set; }

    [System.NonSerialized]
    public bool m_collisionTreated;

    public void setVolume(float _volume)
    {
        m_volume = _volume;
        float radius = Mathf.Pow((3 * m_volume) / (4 * Mathf.PI), 1.0f / 3.0f);
        transform.localScale = new Vector3(radius, radius, radius);
    }

    public void setMinVolume(float _minVolume)
    {
        m_stretchRatio = _minVolume * m_dropTarget.m_initialVelocity;
    }

    // Use this for initialization
    void Awake()
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
        if (m_dropMovement.m_underControl)
        {
            // Equation to respect otherwise stretch is needed
            float newVolume = m_stretchRatio / m_dropMovement.m_velocity.magnitude;
            if (m_volume - newVolume > newVolume)
            {
                float vol = (m_volume - newVolume) / 2.0f;
                if (m_volume - (newVolume + vol) > newVolume)
                    newVolume += vol;

                stretch(newVolume);
            }
        }
    }

    void LateUpdate()
    {
        m_collisionTreated = false;
    }

    // Spawn a new drop smaller and reducing the current radius
    private void stretch(float _volume)
    {
        //Debug.Break();

//         GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//         sphere.transform.position = transform.position;
//         sphere.transform.localScale = transform.localScale;

        Drop newSmallerDrop = GameObject.Instantiate<Transform>(m_waterProjectile.m_dropPrefab).GetComponent<Drop>();
        m_waterProjectile.m_dropPool.Add(newSmallerDrop);

        newSmallerDrop.GetComponent<DropVolume>().init(m_waterProjectile, _volume);

        Vector3 position = transform.position + m_dropMovement.m_velocity.normalized * transform.localScale.x / 2.0f
                                              - m_dropMovement.m_velocity.normalized * newSmallerDrop.transform.localScale.x / 2.0f;
        newSmallerDrop.init(m_waterProjectile, position, m_dropMovement.m_underControl, ++Drop.s_id);
        newSmallerDrop.m_velocity = m_dropMovement.m_velocity;

        DropTarget newDropTarget = newSmallerDrop.GetComponent<DropTarget>();
        newDropTarget.enabled = m_dropMovement.m_underControl;
        if (m_dropMovement.m_underControl)
        {
            newDropTarget.Init(m_dropTarget.m_target, newSmallerDrop.m_velocity);
        }

        float oldRadius = transform.localScale.x / 2.0f;
        setVolume(m_volume - _volume);
        transform.position = transform.position - m_dropMovement.m_velocity.normalized * oldRadius
                                                + m_dropMovement.m_velocity.normalized * transform.localScale.x / 2.0f;
    }

    void OnTriggerStay(Collider _collider)
    {
        DropVolume dropVolume = _collider.GetComponent<DropVolume>();
        if (dropVolume && m_dropMovement.m_underControl)
        {
            if (m_waterProjectile == dropVolume.m_waterProjectile)
            {
                // Merge drops
                if (m_collisionTreated || dropVolume.m_collisionTreated)
                {
                    return;
                }

                DropVolume nearestToTarget;
                DropVolume farthestToTarget;

                float distThis = Vector3.Distance(m_dropTarget.m_target.transform.position, transform.position);
                float distOther = Vector3.Distance(dropVolume.m_dropTarget.m_target.transform.position, dropVolume.transform.position);
                if (distThis <= distOther)
                {
                    nearestToTarget = this;
                    farthestToTarget = dropVolume;
                }
                else
                {
                    nearestToTarget = dropVolume;
                    farthestToTarget = this;
                }

                if (Vector3.Distance(nearestToTarget.transform.position, farthestToTarget.transform.position) < nearestToTarget.transform.localScale.x / 4.0f)
                {
                    nearestToTarget.setVolume(nearestToTarget.m_volume + farthestToTarget.m_volume);
                    farthestToTarget.m_dropMovement.destroy();
                }

                // Destroy happen once all triggers are treated, so we need to prevent merge from the smallest one
                m_collisionTreated = true;
                dropVolume.m_collisionTreated = true;
            }
            return;
        }
    }

    float getDistanceToTarget()
    {
        Vector3 AB = m_dropTarget.m_target.transform.position - transform.position;
        float dist = AB.magnitude;
        Vector3 v = Vector3.Project(m_dropMovement.m_velocity, AB.normalized);
        if (v.normalized == AB.normalized)
            return -dist;
        else
            return dist;
    }
}
