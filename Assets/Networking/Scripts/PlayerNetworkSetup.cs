using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkSetup : NetworkBehaviour {

	private void enableScript<T>() where T : MonoBehaviour {
		T t = GetComponent<T>();
		if (t != null) t.enabled = true;
	}

	//public void Start() {
	public override void OnStartLocalPlayer() {
		//if (isLocalPlayer) {
		enableScript<Move>();
		enableScript<Rotate>();
		enableScript<ComputeActionsFromInput>();

		//Network animator
		Renderer[] rdrs = GetComponentsInChildren<Renderer>();
		foreach (Renderer rdr in rdrs) {
			rdr.enabled = false;
		}
		GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
	}

	public override void PreStartClient() {
		GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
	}
}
