using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Drop))]
public class DropVolume : MonoBehaviour
{
    Drop m_dropMovement;
    DropTarget m_dropTarget;
    WaterGroup m_waterGroup;

    public float m_volume { get; private set; }

    public float m_stretchRatio { get; private set; }

    [System.NonSerialized]
    public bool m_collisionTreated;

    private float m_minVolume;

    public void setVolume(float _volume)
    {
        m_volume = _volume;
        float radius = Mathf.Pow((3 * m_volume) / (4 * Mathf.PI), 1.0f / 3.0f);
        transform.localScale = new Vector3(radius, radius, radius);
    }

    public void setMinVolume(float _minVolume)
    {
        m_minVolume = _minVolume;
        m_stretchRatio = _minVolume * m_dropTarget.m_initialVelocity;
    }

    // Use this for initialization
    void Awake()
    {
        m_dropMovement = GetComponent<Drop>();
        m_stretchRatio = 2.5f;
    }

    public void init(WaterGroup _waterProjectile, float _minVolume, float _volume)
    {
        m_waterGroup = _waterProjectile;
        m_dropTarget = GetComponent<DropTarget>();
        setMinVolume(_minVolume);
        setVolume(_volume);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_dropTarget)
        {
            // Equation to respect otherwise stretch is needed
            float newVolume = m_stretchRatio / m_dropMovement.velocity.magnitude;
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

        Drop newSmallerDrop = GameObject.Instantiate<Transform>(m_waterGroup.m_dropPrefab).GetComponent<Drop>();
        m_waterGroup.m_dropPool.Add(newSmallerDrop);

        Vector3 position = transform.position + m_dropMovement.velocity.normalized * transform.localScale.x / 2.0f
                                              - m_dropMovement.velocity.normalized * newSmallerDrop.transform.localScale.x / 2.0f;
        newSmallerDrop.init(position, m_waterGroup);
        newSmallerDrop.initVelocity(m_dropMovement.velocity);

        newSmallerDrop.gameObject.AddComponent<DropTarget>();
        newSmallerDrop.m_dropTarget = newSmallerDrop.GetComponent<DropTarget>();
        newSmallerDrop.m_dropTarget.Init(m_dropTarget.m_target, newSmallerDrop.velocity);

        newSmallerDrop.GetComponent<DropVolume>().init(m_waterGroup, m_minVolume, _volume);

        newSmallerDrop.gameObject.AddComponent<DropHover>();
        DropHover dropHover = newSmallerDrop.GetComponent<DropHover>();
        dropHover.m_hoverFeature = true;
        dropHover.m_stopFeature = true;

        float oldRadius = transform.localScale.x / 2.0f;
        setVolume(m_volume - _volume);
        transform.position = transform.position - m_dropMovement.velocity.normalized * oldRadius
                                                + m_dropMovement.velocity.normalized * transform.localScale.x / 2.0f;
    }

    void OnTriggerStay(Collider _collider)
    {
        DropVolume colliderDropVolume = _collider.GetComponent<DropVolume>();
        if (colliderDropVolume && m_dropTarget)
        {
            if (m_waterGroup && m_waterGroup == colliderDropVolume.m_waterGroup)
            {
                // Merge drops
                if (m_collisionTreated || colliderDropVolume.m_collisionTreated)
                {
                    return;
                }

                DropVolume nearestToTarget;
                DropVolume farthestToTarget;

                float distThis = Vector3.Distance(m_dropTarget.m_target.transform.position, transform.position);
                float distOther = Vector3.Distance(colliderDropVolume.m_dropTarget.m_target.transform.position, colliderDropVolume.transform.position);
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

                if (Vector3.Distance(nearestToTarget.transform.position, farthestToTarget.transform.position) < nearestToTarget.transform.localScale.x / 4.0f)
                {
                    nearestToTarget.setVolume(nearestToTarget.m_volume + farthestToTarget.m_volume);
                    Destroy(farthestToTarget.m_dropMovement);
                }

                // Destroy happen once all triggers are treated, so we need to prevent merge from the smallest one
                m_collisionTreated = true;
                colliderDropVolume.m_collisionTreated = true;
            }
            return;
        }
    }

    float getDistanceToTarget()
    {
        Vector3 AB = m_dropTarget.m_target.transform.position - transform.position;
        float dist = AB.magnitude;
        Vector3 v = Vector3.Project(m_dropMovement.velocity, AB.normalized);
        if (v.normalized == AB.normalized)
            return -dist;
        else
            return dist;
    }
}
