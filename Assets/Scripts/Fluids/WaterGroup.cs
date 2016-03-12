using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

// WaterGroup only on the server
public class WaterGroup : NetworkBehaviour
{

    public Transform m_dropPrefab;

    [System.NonSerialized]
    public List<Drop> m_dropPool = new List<Drop>();
// 
//     float m_alpha;
//     float m_beta;

    GameObject m_target;

    public float distToUpdateTarget = 0.5f;

    [Server]
    public void init(WaterReserve _waterReserve, float _minVolume, float _volumeWanted, GameObject _target, float _speed)
    {
        m_target = _target;

        Drop drop = _waterReserve.pullWater(_volumeWanted);
        drop.init(transform.position, this);

        DropVolume dropVolume = drop.GetComponent<DropVolume>();
        dropVolume.init(this, _speed, _minVolume, dropVolume.m_volume);

        NetworkServer.Spawn(drop.gameObject);
        //DropSync dropSync = drop.GetComponent<DropSync>();
        //dropSync.RpcInit();

        // Velocity not synchronised
        drop.initVelocity((m_target.transform.position - transform.position).normalized * _speed);
        
        drop.gameObject.AddComponent<DropPullEffector>();
        drop.GetComponent<DropPullEffector>().init(_target, _speed);

        m_dropPool.Add(drop);
    }

    [Server]
    void Update()
    {
        if (m_dropPool.Count == 0)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

//     void computeAngles()
//     {
//         m_alpha = 0;
//         m_beta = 0;
//     }

    [Server]
    public void turnAround(float _radiusToTurnAround)
    {
        foreach (Drop drop in m_dropPool)
        {
            drop.removeEffectorsExceptDropVolume();
            
            DeviationEffector newEffector = drop.gameObject.AddComponent<DeviationEffector>();
            newEffector.init(m_target, _radiusToTurnAround);
        }
    }

    [Server]
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
