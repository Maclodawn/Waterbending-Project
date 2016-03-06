using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkSetup : NetworkBehaviour {

	private void enableScript<T>() where T : MonoBehaviour {
		T t = GetComponent<T>();
		if (t != null) t.enabled = true;
	}

	public void Start() {
		if (isLocalPlayer) {
			enableScript<Move>();
			enableScript<Rotate>();
			enableScript<ComputeActionsFromInput>();
		}
	}
}
