using UnityEngine;
using System.Collections;

public class InformationsLog : MonoBehaviour {

	private GUIText text = null;

	public void Start() {
		text = GetComponent<GUIText>();
	}

	public void Update() {
		
	}

	public void OnGUI() {
		text.pixelOffset = new Vector2(Screen.width-10, Screen.height-10);
	}
}
