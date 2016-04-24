using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyTeamId : MonoBehaviour {

	public Text input;

	public void Start() {
		DontDestroyOnLoad(gameObject);
	}

	public int getTeamId() {
		return int.Parse(input.text.ToString());
	}

	public void hide() {
		enabled = false;
	}

	public int hideAndGetTeamId() {
		hide();
		return getTeamId();
	}
}
