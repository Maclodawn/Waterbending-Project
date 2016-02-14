using UnityEngine;
using System.Collections;

public class WaterReserve : MonoBehaviour
{

    public Transform m_dropPrefab;

    private float m_volume = 0; // Cubic meter

    public void init(Vector3 _position, float _volume)
    {
        transform.position = _position;
        m_volume = _volume;
    }

    public Drop pullWater(float _volume)
    {
        Drop drop;
        if (WaterFlow.s_dropInstancePool.Count == 0)
        {
            drop = GameObject.Instantiate<Transform>(m_dropPrefab).GetComponent<Drop>();
        }
        else
        {
            drop = WaterFlow.s_dropInstancePool[WaterFlow.s_dropInstancePool.Count - 1];
            WaterFlow.s_dropInstancePool.RemoveAt(WaterFlow.s_dropInstancePool.Count - 1);
            drop.gameObject.SetActive(true);
        }

        float volumeDiff = m_volume - _volume;
        if (volumeDiff < 0)
        {
            drop.setVolume(m_volume);
            m_volume = 0;
        }
        else
        {
            drop.setVolume(_volume);
            m_volume = volumeDiff;
        }

        return drop;
    }

    public void Update()
    {
        if (m_volume == 0)
            DestroyObject(gameObject);
    }
}
