using UnityEngine;
using System.Collections;

public class FakePlayer : MonoBehaviour
{

    public bool m_featureDestroyOnContact = true;
    public bool m_featureOscillateBetweenPositions = true;
    public float m_rightOffset = 0;
    public float m_leftOffset = 0;

    public float m_speed;

    Vector3 m_initialPosition;
    bool m_going = true; //true=Left //false=Right

    // Use this for initialization
    void Start()
    {
        m_initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_featureOscillateBetweenPositions)
        {
            if (m_going)
            {
                if (transform.position.x > m_initialPosition.x - m_leftOffset)
                {
                    transform.position = Vector3.Lerp(transform.position, transform.position - transform.right * m_speed, 0.25f);
                }
                else
                {
                    m_going = false;
                }
            }
            else
            {
                if (transform.position.x < m_initialPosition.x + m_rightOffset)
                {
                    transform.position = Vector3.Lerp(transform.position, transform.position + transform.right * m_speed, 0.25f);
                }
                else
                {
                    m_going = true;
                }
            }
        }
    }

    public void OnMyCollisionEnter(GameObject go)
    {
        if (m_featureDestroyOnContact && go.tag == "Drop")
        {
            Destroy(gameObject);
        }
    }
}
