using UnityEngine;
using System.Collections;

public class PlayerLook : MonoBehaviour
{
    private bool m_pause = false;

    public GameObject m_horizontalObj;
    public GameObject m_verticalObj;
    public Vector2 m_rotateSpeed = new Vector2(2, 1);

    [System.NonSerialized]
    public Transform m_playerTransform;
    private float m_distToPlayer;
    private Vector3 m_vectToPlayer;

    public float m_smooth = 3;

	// Use this for initialization
	void Start ()
    {
		Debug.Log(gameObject);
        m_vectToPlayer = transform.position - m_playerTransform.position;
        m_distToPlayer = m_vectToPlayer.magnitude;
    }
	
	void LateUpdate ()
    {
        if (m_pause)
            return;
        float horizontal = (Input.GetAxis("Mouse X") + Input.GetAxis("RightAxisX")) * m_rotateSpeed.x * Manager.getManager().m_cameraSpeed;

        float verticalDir = Manager.getManager().m_yReversed ? 1 : -1;
        float vertical = (Input.GetAxis("Mouse Y") - Input.GetAxis("RightAxisY")) * m_rotateSpeed.y * Manager.getManager().m_cameraSpeed * verticalDir;
        
        m_horizontalObj.transform.Rotate(0, horizontal, 0);
        m_verticalObj.transform.Rotate(vertical, 0, 0);

		transform.position = m_playerTransform.position + new Vector3(0, m_vectToPlayer.y, 0) - transform.forward * m_distToPlayer;
	}

    void ReceiveMessage(string msg)
    {
        string str = msg as string;
        if (str == "Pause")
            m_pause = true;
        else if (str == "UnPause")
            m_pause = false;
    }
}
