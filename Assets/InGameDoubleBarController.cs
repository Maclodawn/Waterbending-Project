using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameDoubleBarController : MonoBehaviour {

	//main camera on scene: should be set if we want health bars to face camera
	public bool face_camera = true;
	private Camera main_camera = null;

	//on screen display of health: health_bar contains the whole bar and health_img only the life bar
	public Image health_img = null;

	//health component managing health of player
	public HealthComponent health;

	public void Start() {

	}

	public void OnGUI() {
		//life update
		//checking if components are properly set by user
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

		//updating orientation towards camera
		if (main_camera != null) {
			//checking if the three canvas is referenced
			if (gameObject == null) {
				Debug.LogError("Health bar object not found.");
				return;
			}

			gameObject.transform.LookAt(new Vector3(main_camera.transform.position.x,
				main_camera.transform.position.y,
				main_camera.transform.position.z));
		} else if (face_camera)
			main_camera = GameObject.FindObjectOfType<Camera>();

		//power update: TODO
	}
}
