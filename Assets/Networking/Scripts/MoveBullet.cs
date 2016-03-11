using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MoveBullet : NetworkBehaviour {
	
	public Vector3 directionNormalized;
	public float speed;

	public void Start() {
		
	}

	public void Update() {
		if (!isServer)
			return;

		transform.position = transform.position + directionNormalized*speed*Time.deltaTime;
	}
}
