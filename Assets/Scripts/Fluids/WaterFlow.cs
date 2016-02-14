using UnityEngine;
using System.Collections.Generic;

public class WaterFlow : MonoBehaviour
{
    public static List<Drop> s_dropInstancePool = new List<Drop>();

    public Transform m_dropPrefab;
    private Vector3 m_target;
    private Vector3 m_updatedTarget;
    [System.NonSerialized]
    public List<Drop> m_dropPool = new List<Drop>();

    [System.NonSerialized]
    public float m_alpha = 0, m_beta = 0, m_deltaAlpha = 0, m_deltaBeta = 0;
    private float m_speed = 6;

    private float m_volume;

    public void init(WaterReserve _reserve, Vector3 _endPos, float _volumeWanted)
    {
        transform.position = _reserve.transform.position;
        m_updatedTarget = _endPos;
        m_target = _endPos;

        m_dropPool.Add(_reserve.pullWater(_volumeWanted));

        m_volume = m_dropPool[0].getVolume();

        m_dropPool[0].init(this, m_volume, Vector3.forward * m_speed, false, true, 0);
        m_dropPool[0].transform.position = transform.position;
        m_dropPool[0].updateTarget(m_target);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("WaterFlow.s_dropInstancePool.Count=" + WaterFlow.s_dropInstancePool.Count);
        Debug.Log("m_dropPool.Count=" + m_dropPool.Count);
        if (Vector3.Distance(m_updatedTarget, m_target) > 0.5f)
        {
            m_target = m_updatedTarget;
            foreach (Drop drop in m_dropPool)
            {
                drop.m_velocity = Vector3.forward * m_speed;
                drop.updateTarget(m_target);
            }
        }
    }

    void LateUpdate()
    {
        if (m_dropPool.Count == 0)
            DestroyObject(gameObject);
    }

    public void updateTarget(Vector3 _target)
    {
        m_updatedTarget = _target;
    }

    public void releaseControl()
    {
        foreach (Drop drop in m_dropPool)
        {
            drop.releaseControl();
        }
    }
}
