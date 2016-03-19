﻿using UnityEngine;
using System.Collections;

public class WaterReserve : MonoBehaviour
{

    public float m_radius = 2;

    void Start()
    {
        transform.localScale = new Vector3(m_radius, transform.localScale.y, m_radius);
    }

    public void init(Vector3 _position)
    {
        transform.position = _position;
    }

    public Drop pullWater()
    {
        Destroy(gameObject);
        Drop drop = Instantiate(Manager.getManager().m_dropPrefab).GetComponent<Drop>();
        Instantiate(Manager.getManager().m_dropParticlesPrefab).GetComponent<DropParticles>().drop = drop;
        return drop;
    }
}
