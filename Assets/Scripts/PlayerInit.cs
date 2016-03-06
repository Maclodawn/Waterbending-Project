using UnityEngine;
using System.Collections;

public class PlayerInit : MonoBehaviour {

	Manager mgr = null;
	Camera cam = null;

	public GameObject m_mainCamera = null;

	public void Start() {
		mgr = Manager.getInstance();
		mgr.addPlayer(gameObject);
		cam = GetComponentInChildren<Camera>();

		//Gab's solution
		GameObject camera = Instantiate(m_mainCamera);
		PlayerLook playerLook = camera.GetComponentInChildren<PlayerLook>();
		playerLook.m_playerTransform = gameObject.transform;
		GetComponent<ComputeActionsFromInput>().m_cameraTransform = camera.transform;

		//PlayerLook playerLook = cam.GetComponentInChildren<PlayerLook>();
		//playerLook.m_playerTransform = transform;
		//GetComponent<ComputeActionsFromInput>().m_cameraTransform = cam.transform;
	}
}