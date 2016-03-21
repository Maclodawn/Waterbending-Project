using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MoveBullet : NetworkBehaviour {
	
	[SyncVar] private Vector3 directionNormalized;
	public float speed;
	public GameObject myPlayer;

	public void Start() {
		if (isServer)
			directionNormalized = myPlayer.transform.forward;
	}

	public void Update() {
		if (isServer)
			transform.position = transform.position + directionNormalized*speed*Time.deltaTime;
	}
}
