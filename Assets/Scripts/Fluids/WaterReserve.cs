using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WaterReserve : NetworkBehaviour
{

    public float m_radius = 2;

    void Start()
    {
        transform.localScale = new Vector3(m_radius, transform.localScale.y, m_radius);
    }

    [Server]
    public void init(Vector3 _position, float _volume)
    {
        transform.position = _position;
        GetComponent<WaterReserveSync>().initDone = true;
    }

    [Server]
    public Drop pullWater(float _volume)
    {
        NetworkServer.Destroy(gameObject);
        Drop drop = Instantiate(Manager.getManager().m_dropPrefab).GetComponent<Drop>();
        Instantiate(Manager.getManager().m_dropParticlesPrefab).GetComponent<DropParticles>().drop = drop;
        return drop;
    }
}
