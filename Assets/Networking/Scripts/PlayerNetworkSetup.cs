using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkSetup : NetworkBehaviour {

	private T enableScript<T>() where T : MonoBehaviour {
		T t = GetComponent<T>();
		if (t != null) t.enabled = true;
		return t;
	}

	public void Start() {
		if (isLocalPlayer) {
			enableScript<Move>();
			enableScript<Rotate>();
			ComputeActionsFromInput script = enableScript<ComputeActionsFromInput>();

			if (script != null)
				script.init();
		}
	}
}
