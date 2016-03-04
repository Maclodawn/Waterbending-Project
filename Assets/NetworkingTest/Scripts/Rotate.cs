using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public float angularSpeed;

	public void Update () {
		float mouseX = Input.GetAxis("Mouse X");
		//float mouseY = Input.GetAxis("Mouse Y");

		//transform.Rotate(-mouseY * Time.deltaTime * angularSpeed, mouseX * Time.deltaTime * angularSpeed, 0f);
		transform.Rotate(0f, mouseX * Time.deltaTime * angularSpeed, 0f);
	}
}
