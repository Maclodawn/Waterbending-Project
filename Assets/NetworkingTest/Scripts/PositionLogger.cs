using UnityEngine;
using System.Collections;

public class PositionLogger : MonoBehaviour {

	public void Update() {
		Debug.Log("Pos: " + transform.position);
	}
}
