using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BulletSyncPosition : NetworkBehaviour {

	//Commands are called by server
	//ClientCallbacks are called by clients

	[SyncVar] private Vector3 syncPos; //Transmits value to all clients when changes
	[SerializeField] Transform myTransform;
	[SerializeField] float lerpRate = 15f;

	public void Start() {
		//myPlayer = GetComponent<MoveBullet>().myPlayer.GetComponent<NetworkBehaviour>();
	}

	public void FixedUpdate() {
		RpcTransmitPosition();
	}

	//Called by client, executed on server
	[ServerCallback] private void ProvidePositionToClients(Vector3 Pos) {
		myTransform.position = Vector3.Lerp(myTransform.position, Pos, Time.deltaTime * lerpRate);
	}

	//Only executed by clients
	[ClientRpc] private void RpcTransmitPosition() {
		ProvidePositionToClients(myTransform.position);
	}
}
