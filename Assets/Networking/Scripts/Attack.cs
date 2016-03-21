using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Attack : NetworkBehaviour {
	
	public GameObject bulletPrefab;

	public void Update() {
		if (Input.GetKeyDown("space")) {
			CmdShoot();
		}
	}

	[Command]
	private void CmdShoot() {
		GameObject bullet = (GameObject) Instantiate(bulletPrefab, transform.position+transform.forward, Quaternion.identity);
		bullet.GetComponent<MoveBullet>().myPlayer = gameObject;
		NetworkServer.Spawn(bullet);
	}
}
