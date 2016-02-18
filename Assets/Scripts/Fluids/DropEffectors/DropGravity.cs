using UnityEngine;
using System.Collections;

public class DropGravity : MonoBehaviour {
    private Drop m_drop;
    public float gravity;

	// Use this for initialization
	void Awake()
    {
        m_drop = GetComponent<Drop>();
    }

    void FixedUpdate()
    {
        m_drop.AddForce(gravity * Vector3.down * Time.fixedDeltaTime);
    }
}
