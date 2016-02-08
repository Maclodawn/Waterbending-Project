using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkSetup : NetworkBehaviour {

	public void Start() {
		if (isLocalPlayer) {
			GetComponent<Move>().enabled = true;
			GetComponent<Rotate>().enabled = true;
		}
	}
}
