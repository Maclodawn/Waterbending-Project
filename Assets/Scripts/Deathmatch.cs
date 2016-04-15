﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Deathmatch : MonoBehaviour {

	private LinkedList<HealthComponent> players_alive = null;
	private LinkedList<HealthComponent> players_dead = null;
	private HealthComponent my_player = null;

	//Initializations
	public void Start () {
		players_alive = new LinkedList<HealthComponent>();
		players_dead = new LinkedList<HealthComponent>();

		//Retrieving my own player
		my_player = gameObject.GetComponentInParent<HealthComponent>();
		if (my_player == null)
			throw new ArgumentException("HealthComponent missing in player.");

		//Retrieves all players
		GameObject[] go_players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject tmp_go_player in go_players) {
			//we're only interested in their healths
			HealthComponent tmp_health = tmp_go_player.GetComponent<HealthComponent>();

			//throwing exception because it is not normal
			if (tmp_health == null)
				throw new Exception("HealthComponent missing in player.");
			
			if (isAlive(tmp_health))
				players_alive.AddFirst(tmp_health);
			else
				players_dead.AddFirst(tmp_health);
		}
	}

	//At each frame, checks if a new player is dead...
	//We assume we can only die once!
	public void Update () {
		//find dead players in players_alive and add them to players_dead
		foreach (HealthComponent tmp_player in players_alive) {
			if (!isAlive(tmp_player))
				players_dead.AddFirst(tmp_player);
		}

		//then remove them from players_alive
		foreach (HealthComponent tmp_player in players_dead) {
			if (players_alive.Contains(tmp_player))
				players_alive.Remove(tmp_player);
			else break; //We assume we go through the list in order and newly dead players are upfront;
						//therefore we don't need to check the rest of the list 
		}

		//you're dead if your current player is dead
		if (players_dead.Contains(my_player))
			Debug.LogWarning("YOU'RE DEAD"); //TODO GUI message on screen (loss)
		
		//victory detection
		else if (players_alive.Count < 2 && players_alive.Contains(my_player))
			Debug.LogWarning("YOU WIN!"); //TODO GUI message on screen (victory)
		else
			Debug.LogWarning(my_player.Health + " " + players_alive.Count + " " + players_dead.Count);
	}

	//TODO implement a way to see if _player is still alive or not
	private bool isAlive(HealthComponent player_health) {
		return player_health.Health > -1;
	}
}
