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
	void Update ()
    {
        for(int i=drops.Count-1; i >= 0; i--)
        {
            if(drops[i] != null)
            {
                GameObject obj = drops[i].gameObject;
                print("DROP TEST");
                Destroy(obj);
                drops.RemoveAt(i);
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Drop")
        {
            drops.Add(other.GetComponent<Drop>());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Drop")
        {
            Drop drop = other.GetComponent<Drop>();
            drops.Remove(drop);
        }
    }
}
