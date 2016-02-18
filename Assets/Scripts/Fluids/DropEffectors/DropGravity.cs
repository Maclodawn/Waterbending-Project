using UnityEngine;
using System.Collections;

public class DropGravity : MonoBehaviour {
    private Drop m_drop;
    public Vector3 m_gravity;

	// Use this for initialization
	void Awake()
    {
        m_drop = GetComponent<Drop>();
    }

    void FixedUpdate()
    {
        m_drop.AddForce(m_gravity);
    }
}
