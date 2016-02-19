using UnityEngine;
using System.Collections.Generic;

public class WaterGroup : MonoBehaviour
{

    public Transform m_dropPrefab;

    [System.NonSerialized]
    public List<Drop> m_dropPool = new List<Drop>();

    Vector3 m_initialVelocity;

    float m_alpha;
    float m_beta;

    public void init(WaterReserve _waterReserve, float _minVolume, float _volumeWanted, GameObject _target, float _speed)
    {
        Drop drop = _waterReserve.pullWater(_volumeWanted);
        drop.init(transform.position, this);
        
        drop.gameObject.AddComponent<DropTarget>();
        drop.m_dropTarget = drop.GetComponent<DropTarget>();
        drop.m_dropTarget.GetComponent<DropTarget>().Init(_target, _speed, m_alpha, m_beta);
        
        DropVolume dropVolume = drop.GetComponent<DropVolume>();
        dropVolume.init(this, _minVolume, dropVolume.m_volume);
        
        drop.gameObject.AddComponent<DropHover>();
        DropHover dropHover = drop.GetComponent<DropHover>();
        dropHover.m_hoverFeature = true;
        dropHover.m_stopFeature = true;

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
        //Debug.Break();
        foreach (Drop drop in m_dropPool)
        {
            drop.releaseControl();
        }
    }
}
