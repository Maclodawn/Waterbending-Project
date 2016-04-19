using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameHealthBarController : MonoBehaviour {

	//on screen display of health
	public Image health_img;

	//health component managing health of player
	public HealthComponent health;

	public void OnGUI() {
		//checking if component are properly set by user
		if (health_img == null) {
			Debug.LogError("Health image component not found.");
			return;
		}

		if (health == null) {
			Debug.LogError("Health component not found.");
			return;
		}

		//update of health image from health component
		health_img.transform.localScale = new Vector3(health.Health/health.MaxHealth,
														health_img.transform.localScale.y,
														health_img.transform.localScale.z);
	}
}
