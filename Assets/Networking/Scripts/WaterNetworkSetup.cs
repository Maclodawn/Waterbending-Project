using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WaterNetworkSetup : NetworkBehaviour {

	private T enableScript<T>() where T : MonoBehaviour {
		T t = GetComponent<T>();
		if (t != null) t.enabled = true;
		return t;
	}

	public void Start() {
		if (isLocalPlayer) {
			enableScript<Drop>();
			enableScript<WaterReserve>();
			enableScript<WaterGroup>();
		}
	}
}

