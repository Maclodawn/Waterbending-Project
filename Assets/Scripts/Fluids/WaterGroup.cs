using UnityEngine;
using System.Collections.Generic;

public class WaterGroup : MonoBehaviour
{

    [System.NonSerialized]
    public List<Drop> m_dropPool = new List<Drop>();
// 
//     float m_alpha;
//     float m_beta;

    public GameObject m_target { get; private set; }

    public float distToUpdateTarget = 0.5f;

    public bool m_flingingFromSelect { get; private set; }
    public bool m_flingingFromTurn { get; private set; }
    private float m_flingSpeed;
    private Vector3 m_posToFling;
#pragma warning disable 0414
    private GameObject m_oldTarget;

    float m_alpha;

    //--
    float m_volumeToSpawn;
    float m_minVolume;
    float m_speed;
    WaterReserve m_waterReserve;
    
    void Awake()
    {
        m_target = new GameObject();
        m_target.name = "WaterTarget";
        m_flingingFromSelect = false;
        m_flingingFromTurn = false;
    }

    public void setTarget(GameObject _target)
    {
        if (m_oldTarget && m_oldTarget.name == "WaterTarget")
            Destroy(m_oldTarget);
        m_oldTarget = m_target;
        m_target = _target;
    }

    public void initPull(Vector3 _position, WaterReserve _waterReserve, float _minVolume, float _volumeWanted, Vector3 _targetPosition, float _speed)
    {
        transform.position = _position;
        m_target.transform.position = _targetPosition;

        Drop drop = _waterReserve.pullWater(_volumeWanted);
        drop.init(transform.position, this);
        drop.initVelocity((m_target.transform.position - transform.position).normalized * _speed);
        
        drop.gameObject.AddComponent<DropPullEffector>();
        drop.GetComponent<DropPullEffector>().init(m_target, _speed);

        DropVolume dropVolume = drop.GetComponent<DropVolume>();
        dropVolume.init(this, _speed, _minVolume, dropVolume.m_volume);

        m_dropPool.Add(drop);
    }

    public void initSelect(Vector3 _position, WaterReserve _waterReserve, float _minVolume, float _volumeWanted, float _speed)
    {
        transform.position = _position;

        m_waterReserve = _waterReserve;
        m_minVolume = _minVolume;
        m_volumeToSpawn = _volumeWanted;
        m_speed = _speed;

        m_volumeToSpawn -= m_minVolume;
        Drop drop = _waterReserve.pullWater(_minVolume);
        drop.init(transform.position, this);

        DropVolume dropVolume = drop.GetComponent<DropVolume>();
        dropVolume.init(this, _speed, _minVolume, dropVolume.m_volume);

        m_dropPool.Add(drop);
    }

    void Update()
    {
        if (m_dropPool.Count == 0)
            Destroy(gameObject);

        if (m_flingingFromTurn)
        {
            m_flingingFromTurn = false;
            foreach (Drop drop in m_dropPool)
            {
                if (drop.GetComponent<DropTarget>())
                    continue;

                m_flingingFromTurn = true;
                RotateEffector dropRotateEffector = drop.GetComponent<RotateEffector>();
                DeviationEffector dropDeviationEffector = drop.GetComponent<DeviationEffector>();

                if (!dropRotateEffector && !dropDeviationEffector
                    || (/*drop.GetComponent<RotateEffector>() && */Vector3.Distance(drop.transform.position, m_posToFling) < 0.4f))
                {
                    drop.removeEffectors();

                    DropTarget newEffector = drop.gameObject.AddComponent<DropTarget>();
                    newEffector.init(m_target, m_flingSpeed, m_alpha, 0);
                }
            }
        }
        else if (m_flingingFromSelect)
        {
            if (!m_dropPool[m_dropPool.Count - 1].GetComponent<DropTarget>())
            {
                Drop drop = m_dropPool[m_dropPool.Count - 1].GetComponent<Drop>();
                drop.removeEffectors();
                DropTarget newEffector = drop.gameObject.AddComponent<DropTarget>();
                newEffector.init(m_target, m_flingSpeed, m_alpha, 0);
            }

            while (m_volumeToSpawn > 0 && m_dropPool[m_dropPool.Count - 1].GetComponent<DropTarget>()
                && Vector3.Distance(m_dropPool[m_dropPool.Count - 1].transform.position, transform.position)
                                > m_dropPool[m_dropPool.Count - 1].transform.localScale.x / 4.0f)
            {
                m_volumeToSpawn -= m_minVolume;
                Drop drop = m_waterReserve.pullWater(m_minVolume);
                drop.init(transform.position, this);

                DropVolume dropVolume = drop.GetComponent<DropVolume>();
                dropVolume.init(this, m_speed, m_minVolume, dropVolume.m_volume);

                DropTarget newEffector = drop.gameObject.AddComponent<DropTarget>();
                newEffector.init(m_target, m_flingSpeed, m_alpha, 0);

                m_dropPool.Add(drop);
                if (m_volumeToSpawn <= 0)
                    m_flingingFromSelect = false;
            }
        }
    }

    void computeAngles()
    {
//         m_alpha = 0;
//         m_beta = 0;
    }

    public void stopAndTurnAround(float _radiusToTurnAround)
    {
        foreach (Drop drop in m_dropPool)
        {
            drop.AddForce(-drop.velocity);
            drop.removeEffectors();
            
            DeviationEffector newEffector = drop.gameObject.AddComponent<DeviationEffector>();
            newEffector.init(m_target, _radiusToTurnAround);
        }
    }

    public void flingFromSelect(float _speed, Vector3 _posToFling, float _alpha)
    {
        m_flingSpeed = _speed;
        m_flingingFromSelect = true;
        m_posToFling = _posToFling;
        m_alpha = _alpha;
    }

    public void flingFromTurn(float _speed, Vector3 _posToFling, float _alpha)
    {
        m_flingSpeed = _speed;
        m_flingingFromTurn = true;
        m_posToFling = _posToFling;
        m_alpha = _alpha;
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

    void OnDestroy()
    {
        if (m_target && m_target.name == "WaterTarget")
            Destroy(m_target);
        if (m_oldTarget && m_oldTarget.name == "WaterTarget")
            Destroy(m_oldTarget);
    }
}
