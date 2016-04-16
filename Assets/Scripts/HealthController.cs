using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;

public class HealthController : NetworkBehaviour {

	private HealthComponent health = null;

	public void Start() {
		//retrieving health component
		health = gameObject.GetComponent<HealthComponent>();
		if (health == null)
			throw new ArgumentException("Health Component not found.");
	}

	[ServerCallback]
	public void OnTriggerEnter(Collider collider) {
		applyDamage(collider);
	}

	//when collision, apply damages to player's health component
	[Server]
	public void applyDamage(Collider collider) {
		if (collider.gameObject.tag.Contains("Drop")) {
			health.Health -= 10; //TODO way of computing damage=f(power)?
			if (health.Health < 1) {
				//you're dead if your current player is dead
				Debug.LogError(gameObject.name + " IS DEAD"); //TODO GUIText message... How to control network...
				NetworkServer.Destroy(gameObject);
			}
		}
	}
}
