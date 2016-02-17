using UnityEngine;
using System.Collections.Generic;

public class WaterProjectile : MonoBehaviour
{

    public Transform m_dropPrefab;

    [System.NonSerialized]
    public List<Drop> m_dropPool = new List<Drop>();

    Vector3 m_initialVelocity;

    float m_alpha;
    float m_beta;

    public void init(WaterReserve _waterReserve, float _volumeWanted, GameObject _target, float _speed)
    {
        Drop drop = _waterReserve.pullWater(_volumeWanted);
        drop.init(this, transform.position, true, 0);
        drop.GetComponent<DropTarget>().Init(_target, _speed, m_alpha, m_beta);
        DropVolume dropVolume = drop.GetComponent<DropVolume>();
        dropVolume.init(this, dropVolume.m_volume);
        m_dropPool.Add(drop);
    }

    void Update()
    {
        if (m_dropPool.Count == 0)
            Destroy(gameObject);
    }

    void computeAngles()
    {
        m_alpha = 0;
        m_beta = 0;
    }

    public void releaseControl()
    {
        foreach (Drop drop in m_dropPool)
        {
            drop.m_underControl = false;
        }
    }
}
