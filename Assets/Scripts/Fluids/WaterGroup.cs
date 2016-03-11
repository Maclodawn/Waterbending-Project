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

    GameObject m_target;
	public Character m_char {get ; private set;}

	public float distToUpdateTarget = 0.5f;

    public void init(WaterReserve _waterReserve, Character _char, float _minVolume, float _volumeWanted, GameObject _target, float _speed)
    {
        m_target = _target;
		m_char = _char;

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
		//Debug.Break();
        if (m_dropPool.Count == 0)
            Destroy(gameObject);
    }

    void computeAngles()
    {
//         m_alpha = 0;
//         m_beta = 0;
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
