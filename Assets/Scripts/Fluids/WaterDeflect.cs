using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class WaterDeflect : MonoBehaviour
{
    public float m_avoidRadius, m_deflectRange, m_duration;
    private float m_time;

    public List<Drop> drops = new List<Drop>();

	// Use this for initialization
	void Start ()
    {
        m_time = m_duration;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        for(int i=drops.Count-1; i >= 0; i--)
        {
            if(drops[i] != null)
            {
                GameObject obj = drops[i].gameObject;
                drops[i].removeEffectors();
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        Drop drop = other.GetComponent<Drop>();
        if (drop != null)
            drops.Add(drop);
    }

    void OnTriggerExit(Collider other)
    {
        Drop drop = other.GetComponent<Drop>();
        if (drop != null)
        {
            drops.Remove(drop);
        }
    }
}
