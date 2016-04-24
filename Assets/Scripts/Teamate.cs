using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Teamate : MonoBehaviour {

	private static int NB_TEAMS = 1;
	public int team_id = -1; //starts at 1, 0 == fakeplayers
	public InformationsLog infos = null;

	public void Start() {
		//retrieving InformationsLog
		infos = GameObject.Find("InformationsLog").GetComponent<InformationsLog>();
	}

	public void Update() {
		
	}

	public bool isFriend(Teamate _teamate) {
		if (!_teamate)
			return false;

		return _teamate.team_id == team_id;
	}

	public void addToTeam(int _new_team_id) {
		team_id = _new_team_id;
		infos.log("<b><color=\"yellow\">" + gameObject.name + "</color></b>: now joining team <b><color=\"yellow\">#" + team_id + "</color></b>");
	}

	public void addToNewTeam() {
		team_id = NB_TEAMS+1;
		++NB_TEAMS;
		infos.log("<b><color=\"yellow\">" + gameObject.name + "</color></b>: now joining team <b><color=\"yellow\">#" + team_id + "</color></b>");
	}
}
