using UnityEngine;
using System.Collections;

public class OptionsOnHold : MonoBehaviour {

	public float m_cameraSpeed = 1f;
	public bool m_yReversed = true;
	public bool m_tuto = true;

	public void Start() {
		DontDestroyOnLoad(gameObject);
	}
}
