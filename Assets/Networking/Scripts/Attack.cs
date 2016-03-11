using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

	public GameObject bulletPrefab;

	public void Update() {
		if (Input.GetKeyDown("Space"))
			GameObject.Instantiate(bulletPrefab);
	}
}
