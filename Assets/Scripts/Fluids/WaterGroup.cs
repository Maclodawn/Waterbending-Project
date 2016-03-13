using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class WaterGroup : NetworkBehaviour
{

    [System.NonSerialized]
    public List<Drop> m_dropPool = new List<Drop>();

    public GameObject m_target { get; private set; }

    public bool m_flingingFromSelect { get; private set; }
    public bool m_flingingFromTurn { get; private set; }
    private float m_flingSpeed;
    private Vector3 m_posToFling;
    private GameObject m_oldTarget;

    float m_alpha;

    float m_volumeToSpawn;
    float m_minVolume;
    float m_speed;
    WaterReserve m_waterReserve;

    public float m_quotient = 1.0f / 4.0f;
    
    void Awake()
    {
        m_flingingFromSelect = false;
        m_flingingFromTurn = false;
    }

    public void setTarget(GameObject _target)
    {
        //FIXME
//         if (m_oldTarget && m_oldTarget.tag == "WaterTarget")
//             NetworkServer.Destroy(m_oldTarget);
        m_oldTarget = m_target;
        m_target = _target;
    }

    [Server]
    public void initPull(Vector3 _position, WaterReserve _waterReserve, float _minVolume, float _volumeWanted, GameObject _target, float _speed)
    {
        transform.position = _position;
        m_target = _target;

        Drop drop = _waterReserve.pullWater(_volumeWanted);
        drop.init(transform.position, this);

        DropVolume dropVolume = drop.GetComponent<DropVolume>();
        dropVolume.init(this, _speed, _minVolume, dropVolume.m_volume);
        
        drop.gameObject.AddComponent<DropPullEffector>();
        drop.GetComponent<DropPullEffector>().init(m_target, _speed);

        NetworkServer.Spawn(drop.gameObject);

        // Velocity not synchronized
        drop.initVelocity((m_target.transform.position - transform.position).normalized * _speed);

        m_dropPool.Add(drop);
    }

    [Server]
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

        NetworkServer.Spawn(drop.gameObject);

        m_dropPool.Add(drop);
    }

    [ServerCallback]
    void Update()
    {
        if (!NetworkServer.active)
            return;

        if (m_dropPool.Count == 0)
        {
            NetworkServer.Destroy(gameObject);
        }

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
                    drop.removeEffectorsExceptDropVolume();

                    DropTarget newEffector = drop.gameObject.AddComponent<DropTarget>();
                    newEffector.init(m_target, m_flingSpeed, m_alpha, 0);
                }
            }
        }
        else if (m_flingingFromSelect && m_dropPool.Count > 0)
        {
            if (!m_dropPool[m_dropPool.Count - 1].GetComponent<DropTarget>())
            {
                Drop drop = m_dropPool[m_dropPool.Count - 1].GetComponent<Drop>();
                drop.removeEffectorsExceptDropVolume();
                DropTarget newEffector = drop.gameObject.AddComponent<DropTarget>();
                newEffector.init(m_target, m_flingSpeed, m_alpha, 0);
            }

            if (m_volumeToSpawn > 0 && m_dropPool[m_dropPool.Count - 1].GetComponent<DropTarget>()
                && Vector3.Distance(m_dropPool[m_dropPool.Count - 1].transform.position, transform.position)
                                > m_dropPool[m_dropPool.Count - 1].transform.localScale.x * m_quotient)
            {
                Drop drop = m_dropPool[m_dropPool.Count - 1];
                float tmpQuotient = 0;
                while (tmpQuotient <= 1)
                {
                    spawn(transform.position + (transform.position - drop.transform.position) * tmpQuotient);
                    tmpQuotient += m_quotient;
                }
            }
        }
    }

    void spawn(Vector3 _position)
    {
        m_volumeToSpawn -= m_minVolume;
        Drop drop = m_waterReserve.pullWater(m_minVolume);
        drop.init(_position, this);

        DropVolume dropVolume = drop.GetComponent<DropVolume>();
        dropVolume.init(this, m_speed, m_minVolume, dropVolume.m_volume);

        DropTarget newEffector = drop.gameObject.AddComponent<DropTarget>();
        newEffector.init(m_target, m_flingSpeed, m_alpha, 0);

        NetworkServer.Spawn(drop.gameObject);

        m_dropPool.Add(drop);
        if (m_volumeToSpawn <= 0 || m_waterReserve.m_volume <= 0)
            m_flingingFromSelect = false;
    }

    [Server]
    public void stopAndTurnAround(float _radiusToTurnAround)
    {
        foreach (Drop drop in m_dropPool)
        {
            drop.AddForce(-drop.velocity);
            drop.removeEffectorsExceptDropVolume();
            
            DeviationEffector newEffector = drop.gameObject.AddComponent<DeviationEffector>();
            newEffector.init(m_target, _radiusToTurnAround);
        }
    }

    [Server]
    public void flingFromSelect(float _speed, Vector3 _posToFling, float _alpha)
    {
        m_flingSpeed = _speed;
        m_flingingFromSelect = true;
        m_posToFling = _posToFling;
        m_alpha = _alpha;
    }

    [Server]
    public void flingFromTurn(float _speed, Vector3 _posToFling, float _alpha)
    {
        m_flingSpeed = _speed;
        m_flingingFromTurn = true;
        m_posToFling = _posToFling;
        m_alpha = _alpha;
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
        NetworkServer.Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (!NetworkServer.active)
            return;

        if (m_target && m_target.tag == "WaterTarget")
        {
            NetworkServer.Destroy(m_target);
        }
        if (m_oldTarget && m_oldTarget.tag == "WaterTarget")
        {
            NetworkServer.Destroy(m_oldTarget);
        }
    }
}
