using UnityEngine;
using System.Collections.Generic;

public class Drop : MonoBehaviour {
    public Vector3 m_gravity;
    private Vector3 m_speed;
    public float initTime = 0.2f;
    

    private List<GameObject> m_initCollisions = new List<GameObject>();

    void OnEnable()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        m_speed += m_gravity * Time.deltaTime;
        transform.position += m_speed * Time.deltaTime;
    }

    void LateUpdate()
    {
        if (initTime > 0)
            initTime -= Time.deltaTime;
    }

    public void SetSpeed(Vector3 speed)
    {
        m_speed = speed;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (initTime > 0)
        {
            m_initCollisions.Add(collider.gameObject);
        }

        if (!m_initCollisions.Contains(collider.gameObject))
            GameObject.Destroy(gameObject);
    }

    void OnTriggerExit(Collider collider)
    {
        if (m_initCollisions.Count > 0 && m_initCollisions.Contains(collider.gameObject))
           m_initCollisions.Remove(collider.gameObject);
    }

    public void SetTarget(Vector3 target, Vector3 speed)
    {
        
    }

    public void SetTarget(Vector3 target)
    {
        SetTarget(target, m_speed);
    }
}
