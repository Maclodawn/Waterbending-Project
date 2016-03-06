using UnityEngine;
using System.Collections;

public class PlayerInit : MonoBehaviour {

	Manager mgr = null;
	Camera cam = null;

	public void Awake() {
		mgr = Manager.getInstance();
		mgr.addPlayer(gameObject);
		cam = GetComponentInChildren<Camera>();

		PlayerLook playerLook = cam.GetComponentInChildren<PlayerLook>();
		playerLook.m_playerTransform = transform;
		GetComponent<ComputeActionsFromInput>().m_cameraTransform = cam.transform;
	}
}