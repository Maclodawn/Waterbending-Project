using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WaterReserve : NetworkBehaviour
{

    public float m_volume;

    [System.NonSerialized][SyncVar]
    public bool isReady = false;

    public void setVolume(float _volume)
    {
        m_volume = _volume;
        //float radius = Mathf.Sqrt(m_volume / (transform.localScale.y * Mathf.PI));
        float radius = 2;
        transform.localScale = new Vector3(radius, transform.localScale.y, radius);
    }

    void Start()
    {
        setVolume(m_volume);
    }

    [Server]
    public void init(Vector3 _position, float _volume)
    {
        transform.position = _position;
        setVolume(_volume);
        GetComponent<WaterReserveSync>().initDone = true;
    }

    [Server]
    public Drop pullWater(float _volume)
    {
        Drop drop = GameObject.Instantiate(Manager.getInstance().m_dropPrefab).GetComponent<Drop>();

        float volumeDiff = m_volume - _volume;
        if (volumeDiff < 0)
        {
            drop.GetComponent<DropVolume>().setVolume(m_volume);
            m_volume = 0;
        }
        else
        {
            drop.GetComponent<DropVolume>().setVolume(_volume);
            m_volume = volumeDiff;
        }

        return drop;
    }

    public void Update()
    {
        if (m_volume == 0 && isReady)
            Destroy(gameObject);
    }
}
