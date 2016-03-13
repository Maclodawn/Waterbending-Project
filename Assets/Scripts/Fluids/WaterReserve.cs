using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WaterReserve : NetworkBehaviour
{

    public float m_volume;

    public void setVolume(float _volume)
    {
        m_volume = _volume;
        float radius = Mathf.Sqrt(m_volume / (transform.localScale.y * Mathf.PI)) / 7.0f;
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

    [ServerCallback]
    public void Update()
    {
        if (!NetworkServer.active)
            return;

        if (m_volume == 0 && NetworkServer.active)
            NetworkServer.Destroy(gameObject);
    }
}
