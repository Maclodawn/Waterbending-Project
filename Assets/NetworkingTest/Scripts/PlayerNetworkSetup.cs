using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkSetup : NetworkBehaviour {

	public void Start() {
		if (isLocalPlayer) {
			Move move = GetComponent<Move>();
			if (move != null) move.enabled = true;

			Rotate rotate = GetComponent<Rotate>();
			if (rotate != null) rotate.enabled = true;

			ComputeActionsFromInput computeActionsFromInput = GetComponent<ComputeActionsFromInput>();
			if (computeActionsFromInput != null) computeActionsFromInput.enabled = true;
		}
	}
}
