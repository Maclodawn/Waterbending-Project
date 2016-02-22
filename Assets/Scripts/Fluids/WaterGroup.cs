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

    GameObject m_target;

    public void init(WaterReserve _waterReserve, float _minVolume, float _volumeWanted, GameObject _target, float _speed)
    {
        m_target = _target;

        Drop drop = _waterReserve.pullWater(_volumeWanted);
        drop.init(transform.position, this);
        
        drop.gameObject.AddComponent<DropTarget>();
        drop.GetComponent<DropTarget>().init(_target, _speed, m_alpha, m_beta);

        DropVolume dropVolume = drop.GetComponent<DropVolume>();
        dropVolume.init(this, _speed, _minVolume, dropVolume.m_volume);

        m_dropPool.Add(drop);
    }

    void Update()
    {
        if (m_dropPool.Count == 0)
            Destroy(gameObject);

        foreach (Drop drop in m_dropPool)
        {
            DropTarget dropTarget = drop.GetComponent<DropTarget>();
            if (dropTarget && Vector3.Distance(drop.transform.position, m_target.transform.position) < 0.5f)
            {
                Destroy(dropTarget);
                drop.gameObject.AddComponent<DropHover>();
                drop.GetComponent<DropHover>().init(m_target, true, true);
            }
        }
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
            drop.m_waterGroup = null;
            drop.removeEffectors();
            drop.gameObject.AddComponent<DropGravity>();
        }
        Destroy(gameObject);
    }
}
