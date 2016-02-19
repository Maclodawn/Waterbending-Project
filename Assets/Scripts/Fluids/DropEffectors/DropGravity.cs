using UnityEngine;
using System.Collections;

public class DropGravity : MonoBehaviour
{
    private Drop m_drop;
    public float gravity;

    void OnEnable()
    {
        m_drop.registerEffector(this);
    }

    // Use this for initialization
    void Awake()
    {
        m_drop = GetComponent<Drop>();
        GameObject obj = GameObject.Find("Manager");
        Manager manager = obj.GetComponent<Manager>();
        gravity = manager.m_waterGravity;
    }

    void FixedUpdate()
    {
        m_drop.AddForce(gravity * Vector3.down * Time.fixedDeltaTime);
    }
}
