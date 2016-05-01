using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WaterReserve : NetworkBehaviour
{

    public float m_radius = 0.15f;

    void Start()
    {
        transform.localScale = new Vector3(m_radius, transform.localScale.y, m_radius);
    }

    [Server]
    public void init(Vector3 _position)
    {
        transform.position = _position;
    }

    [Server]
    public Drop pullWater()
    {
        NetworkServer.Destroy(gameObject);
        Drop drop = Instantiate(Manager.getInstance().m_dropPrefab).GetComponent<Drop>();

        return drop;
    }
}
