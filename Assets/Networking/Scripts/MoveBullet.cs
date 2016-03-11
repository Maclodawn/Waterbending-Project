﻿using UnityEngine;
using System.Collections;

public class MoveBullet : MonoBehaviour {

	public GameObject myPlayer;

	private Vector3 directionNormalized;
	private float speed = 1f;

	public void Start() {
		directionNormalized = myPlayer.transform.forward;
	}

	public void Update() {
		transform.position = transform.position + directionNormalized*speed*Time.deltaTime;
	}
}