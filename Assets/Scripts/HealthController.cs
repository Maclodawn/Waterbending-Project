using UnityEngine;
using System;
using System.Collections;

public class HealthController : MonoBehaviour {

	private HealthComponent health = null;

	public void Start() {
		//retrieving health component
		health = gameObject.GetComponent<HealthComponent>();
		if (health == null)
			throw new ArgumentException("Health Component not found.");
	}

	//TODO check function name
	public void OnCollisionEnter(Collision collision) {
		Debug.LogWarning("OnCollisionEnter");
		applyDamage(collision.collider);
	}

	public void OnTriggerEnter(Collider collider) {
		Debug.LogWarning("OnTriggerEnter");
		applyDamage(collider);
	}

	//when collision, apply damage to player's health component
	private void applyDamage(Collider collider) {
		if (collider.gameObject.name.Contains("Drop")) {
			health.Health -= 10;
		}
	}
}
