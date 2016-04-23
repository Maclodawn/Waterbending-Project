using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class WaterDeflect : MonoBehaviour
{
    public float /*m_radius, */m_minForce, m_maxForce, m_duration, m_splitRatio, m_avoidStrength;
    //private float m_time;

// 	// Use this for initialization
// 	void Start ()
//     {
//         m_time = m_duration;
// 	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {

	}

    void OnTriggerEnter(Collider other)
    {
        Drop drop = other.GetComponent<Drop>();
        if (drop != null)
        {
            drop.removeEffectors();
            drop.gameObject.AddComponent<AvoidEffector>();
            drop.gameObject.GetComponent<AvoidEffector>().init(transform.position/*, m_radius*/, m_splitRatio, m_avoidStrength);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Drop drop = other.GetComponent<Drop>();
        if (drop != null)
        {
            drop.removeEffectors();
            drop.gameObject.AddComponent<DropGravity>();
        }
    }
}
