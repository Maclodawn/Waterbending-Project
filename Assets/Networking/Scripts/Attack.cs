using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

	public GameObject bulletPrefab;

	public void Update() {
		if (Input.GetKeyDown("space")) {
			Object bullet = GameObject.Instantiate(bulletPrefab, transform.position+transform.forward, Quaternion.identity);
			((GameObject) bullet).GetComponent<MoveBullet>().directionNormalized = transform.forward;
		}
	}
}
