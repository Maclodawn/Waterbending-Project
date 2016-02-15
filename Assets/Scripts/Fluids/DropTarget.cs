﻿using UnityEngine;
using System.Collections;

public class DropTarget : MonoBehaviour
{
    private Drop m_drop;
    public GameObject m_target;
    public Vector3 lastTargetPos;
    private float tf, ti;
    public float accelerationCap;

    // Use this for initialization
    void OnEnable ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_target != null)
            UpdateTarget();
	}

    public void Init(GameObject target, Vector3 speed)
    {
        m_target = target;
        m_drop = GetComponent<Drop>();
        Vector3 AB = target.transform.position - transform.position;
        Vector3 x = AB.normalized;
        Vector3 z = Vector3.Cross(x, speed).normalized;
        Vector3 y = Vector3.Cross(z, x);
        float vx = Vector3.Project(speed, x).magnitude;
        float vy = Vector3.Project(speed, y).magnitude;
        float dx = Vector3.Project(AB, x).magnitude;
        float dy = Vector3.Project(AB, y).magnitude;

        tf = AB.magnitude / vx;
        ti = Time.time;

        float g = 2 * vx * vy / AB.magnitude;
        m_drop.m_gravity = -y * g;
        lastTargetPos = m_target.transform.position;
    }

    void UpdateTarget()
    {
        Vector3 acceleration = 2 * (m_target.transform.position - lastTargetPos) / (tf + ti - Time.time) / (tf + ti - Time.time);
        if (acceleration.sqrMagnitude > accelerationCap * accelerationCap)
            acceleration = acceleration.normalized * accelerationCap;
        m_drop.m_gravity += acceleration;
        lastTargetPos = m_target.transform.position;
    }
}
