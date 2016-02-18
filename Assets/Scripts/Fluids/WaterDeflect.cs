using UnityEngine;
using System.Collections.Generic;

public class WaterDeflect : MonoBehaviour
{
    public float m_avoidRadius, m_deflectRange, m_duration;
    private float m_time;

    private List<Drop> drops;

	// Use this for initialization
	void Start ()
    {
        m_time = m_duration;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_deflectRange);

        foreach(Collider collider in colliders)
        {
            if(collider.tag == "Drop")
            {

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
