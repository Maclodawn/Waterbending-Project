using UnityEngine;
using System.Collections.Generic;

public class WaterGroup : MonoBehaviour
{

    public Transform m_dropPrefab;

    [System.NonSerialized]
    public List<Drop> m_dropPool = new List<Drop>();
// 
//     float m_alpha;
//     float m_beta;

    public GameObject m_target { get; private set; }

    public float distToUpdateTarget = 0.5f;

    private bool m_flinging = false;
    private float m_flingSpeed;
    private Vector3 m_posToFling;
    private GameObject m_oldTarget;
    
    public void setTarget(GameObject _target)
    {
        m_oldTarget = m_target;
        m_target = _target;
    }

    public void init(WaterReserve _waterReserve, float _minVolume, float _volumeWanted, GameObject _target, float _speed)
    {
        m_target = _target;

        Drop drop = _waterReserve.pullWater(_volumeWanted);
        drop.init(transform.position, this);
        drop.initVelocity((m_target.transform.position - transform.position).normalized * _speed);
        
        drop.gameObject.AddComponent<DropPullEffector>();
        drop.GetComponent<DropPullEffector>().init(_target, _speed);

        DropVolume dropVolume = drop.GetComponent<DropVolume>();
        dropVolume.init(this, _speed, _minVolume, dropVolume.m_volume);

        m_dropPool.Add(drop);
    }

    void Update()
    {
        if (m_dropPool.Count == 0)
            Destroy(gameObject);

        if (m_flinging)
        {
            foreach (Drop drop in m_dropPool)
            {
                if (drop.GetComponent<RotateEffector>() && Vector3.Distance(drop.transform.position, m_posToFling) < 0.4f)
                {
                    drop.removeEffectorsExceptDropVolume();

                    DropTarget newEffector = drop.gameObject.AddComponent<DropTarget>();
                    newEffector.init(m_target, m_flingSpeed, 0, 0);
                }
            }
        }
    }

    void computeAngles()
    {
//         m_alpha = 0;
//         m_beta = 0;
    }

    public void turnAround(float _radiusToTurnAround)
    {
        foreach (Drop drop in m_dropPool)
        {
            drop.removeEffectorsExceptDropVolume();
            
            DeviationEffector newEffector = drop.gameObject.AddComponent<DeviationEffector>();
            newEffector.init(m_target, _radiusToTurnAround);
        }
    }

    public void fling(float _speed, Vector3 _posToFling)
    {
        m_flingSpeed = _speed;
        m_flinging = true;
        m_posToFling = _posToFling;
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
