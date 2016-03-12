using UnityEngine;
using System.Collections;

public class WaterRotation : MonoBehaviour
{

    public float m_pullForce;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        Drop drop = other.GetComponent<Drop>();
        if (drop != null && drop.GetComponent<RotateEffector>() == null)
        {
            drop.removeEffectors();
            RotateEffector target = drop.gameObject.AddComponent<RotateEffector>();
            target.init(transform.position, Vector3.up/*, m_pullForce*/);
        }
    }

    void OnTriggerExit(Collider other)
    {

    }
}
