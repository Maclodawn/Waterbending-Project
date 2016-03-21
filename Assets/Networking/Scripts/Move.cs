using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	public float speed;

	public void Update () {
		transform.position = transform.position +
			Input.GetAxis("Vertical") * transform.forward * speed * Time.deltaTime;
		
		transform.position = transform.position +
			Input.GetAxis("Horizontal") * transform.right * speed * Time.deltaTime;
	}
}
