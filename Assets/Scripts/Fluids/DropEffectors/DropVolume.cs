using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Drop))]
public class DropVolume : MonoBehaviour
{
    Drop m_drop;
    WaterGroup m_waterGroup;

    public float m_volume { get; private set; }

    public float m_stretchRatio { get; private set; }

    [System.NonSerialized]
    public bool m_collisionTreated;

    private float m_minVolume;

    [System.NonSerialized]
    public float m_initialSpeed;

    private GameObject m_target;

    private int m_counterFrameForMerge = 0;

    public void setVolume(float _volume)
    {
        m_volume = _volume;
        float radius = Mathf.Pow((3 * m_volume) / (4 * Mathf.PI), 1.0f / 3.0f);
        transform.localScale = new Vector3(radius, radius, radius);
    }

    public void setMinVolume(float _minVolume)
    {
        m_minVolume = _minVolume;
        m_stretchRatio = _minVolume * m_initialSpeed;
    }

    // Use this for initialization
    void Awake()
    {
        m_drop = GetComponent<Drop>();
        m_stretchRatio = 2.5f;
    }

    public void init(WaterGroup _waterProjectile, float _initialSpeed, float _minVolume, float _volume)
    {
        m_waterGroup = _waterProjectile;
        m_initialSpeed = _initialSpeed;
        setMinVolume(_minVolume);
        setVolume(_volume);
    }

    void LateUpdate()
    {
        if (!GetComponent<DropGravity>())
        {
            // Equation to respect otherwise stretch is needed
            float newVolume = m_stretchRatio / m_drop.velocity.magnitude;
            if (newVolume >= m_minVolume - 0.1f && m_volume - newVolume > newVolume)
            {
                float vol = (m_volume - newVolume) / 2.0f;
                if (m_volume - (newVolume + vol) > newVolume)
                    newVolume += vol;

                stretch(newVolume);
            }
        }

        m_collisionTreated = false;
    }

    // Spawn a new drop smaller and reducing the current radius
    private void stretch(float _volume)
    {
        Drop newSmallerDrop = GameObject.Instantiate<Transform>(m_waterGroup.m_dropPrefab).GetComponent<Drop>();
        m_waterGroup.m_dropPool.Add(newSmallerDrop);

        Vector3 position = transform.position + m_drop.velocity.normalized * transform.localScale.x / 2.0f
                                              - m_drop.velocity.normalized * newSmallerDrop.transform.localScale.x / 2.0f;
        newSmallerDrop.init(position, m_waterGroup);
        newSmallerDrop.initVelocity(m_drop.velocity);

        if (GetComponent<DropPullEffector>())
        {
            newSmallerDrop.gameObject.AddComponent<DropPullEffector>();
            newSmallerDrop.GetComponent<DropPullEffector>().init(getTarget(), m_initialSpeed);
        }
        else if (GetComponent<DeviationEffector>())
        {
            newSmallerDrop.gameObject.AddComponent<DeviationEffector>();
            newSmallerDrop.GetComponent<DeviationEffector>().init(getTarget(), GetComponent<DeviationEffector>().m_targetRadius);
        }
        else if (GetComponent<RotateEffector>())
        {
            newSmallerDrop.gameObject.AddComponent<RotateEffector>();
            newSmallerDrop.GetComponent<RotateEffector>().init(getTarget(), Vector3.up/*, 1*/, GetComponent<RotateEffector>().m_radiusToTurnAround);
        }

        newSmallerDrop.gameObject.AddComponent<DropVolume>();
        newSmallerDrop.GetComponent<DropVolume>().init(m_waterGroup, m_initialSpeed, m_minVolume, _volume);

        float oldRadius = transform.localScale.x / 2.0f;
        setVolume(m_volume - _volume);
        transform.position = transform.position - m_drop.velocity.normalized * oldRadius
                                                + m_drop.velocity.normalized * transform.localScale.x / 2.0f;
    }

    void OnTriggerStay(Collider _collider)
    {
        DropVolume colliderDropVolume = _collider.GetComponent<DropVolume>();
        if (colliderDropVolume && !GetComponent<DropGravity>())
        {
            if ((m_waterGroup && m_waterGroup == colliderDropVolume.m_waterGroup) || Vector3.Dot(m_drop.velocity, colliderDropVolume.m_drop.velocity) > 0)
            {
                // Merge drops
                if (m_collisionTreated || colliderDropVolume.m_collisionTreated)
                {
                    return;
                }

                DropVolume nearestToTarget;
                DropVolume farthestToTarget;

                float distThis = Vector3.Distance(getTarget().transform.position, transform.position);
                float distOther = Vector3.Distance(colliderDropVolume.getTarget().transform.position, colliderDropVolume.transform.position);
                if (distThis <= distOther)
                {
                    nearestToTarget = this;
                    farthestToTarget = colliderDropVolume;
                }
                else
                {
                    nearestToTarget = colliderDropVolume;
                    farthestToTarget = this;
                }

                if (Vector3.Distance(nearestToTarget.transform.position, farthestToTarget.transform.position) < nearestToTarget.transform.localScale.x / 4.0f /** 3.0f / 8.0f*/
                    || m_counterFrameForMerge > 5)
                {
                    m_counterFrameForMerge = 0;
                    nearestToTarget.setVolume(nearestToTarget.m_volume + farthestToTarget.m_volume);
                    Destroy(farthestToTarget.m_drop.gameObject);
                }
                else if (nearestToTarget.m_drop.velocity == Vector3.zero && farthestToTarget.m_drop.velocity == Vector3.zero)
                    ++m_counterFrameForMerge;
                else
                    m_counterFrameForMerge = 0;

                // Destroy happen once all triggers are treated, so we need to prevent merge from the smallest one
                m_collisionTreated = true;
                colliderDropVolume.m_collisionTreated = true;
            }
            return;
        }
    }

    float getDistanceToTarget()
    {
        Vector3 AB = getTarget().transform.position - transform.position;
        float dist = AB.magnitude;
        Vector3 v = Vector3.Project(m_drop.velocity, AB.normalized);
        if (v.normalized == AB.normalized)
            return -dist;
        else
            return dist;
    }

    GameObject getTarget()
    {
        if (m_target)
            return m_target;

        DropTarget dropTarget = GetComponent<DropTarget>();
        DropPullEffector dropPullEffector = GetComponent<DropPullEffector>();
        DeviationEffector dropDeviationEffector = GetComponent<DeviationEffector>();
        RotateEffector dropRotateEffector = GetComponent<RotateEffector>();
        if (dropTarget)
            m_target = dropTarget.m_target;
        else if (dropPullEffector)
            m_target = dropPullEffector.m_target;
        else if (dropDeviationEffector)
            m_target = dropDeviationEffector.m_target;
        else if (dropRotateEffector)
            m_target = dropRotateEffector.m_target;
        else
        {
            Debug.Break();
            Debug.LogException(new System.Exception("Stretch with no DropTarget, no DropPullEffector,"
                                                  + "no DropDeviationEffector, or no DropRotateEffector"), this);
        }

        return m_target;
    }
}
