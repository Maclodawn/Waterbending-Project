﻿using UnityEngine;
using System.Collections.Generic;

public class Drop/*Movement*/ : MonoBehaviour
{
    private Vector3 m_velocity;
    public float m_initTime = 0.2f;

    private List<GameObject> m_initCollisions = new List<GameObject>();

    public WaterGroup m_waterGroup;

    private DropVolume m_dropVolume;

    private List<MonoBehaviour> m_effectors = new List<MonoBehaviour>();

    public float radius { get { return transform.localScale.x/2; } }

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
}
