using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WaterNetworkSetup : NetworkBehaviour {

	private Drop drop = null;

	private T enableScript<T>() where T : MonoBehaviour {
		T t = GetComponent<T>();
		if (t != null) t.enabled = true;
		return t;
	}

	public void Start() {
		drop = GetComponent<Drop>();
		if (drop != null) {
			if (drop.m_waterGroup.m_char.gameObject.GetComponent<NetworkBehaviour>().isLocalPlayer)
				drop.enabled = true;
		}
	}
}

