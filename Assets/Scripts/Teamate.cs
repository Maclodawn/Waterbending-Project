using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Teamate : MonoBehaviour {

	private static int NB_TEAMS = 0;
	public int team_id = -1; //starts at 1

	public bool isFriend(Teamate _teamate) {
		if (!_teamate)
			return false;

		return _teamate.team_id == team_id;
	}

	public void addToTeam(int _new_team_id) {
		team_id = _new_team_id;
	}

	public void addToNewTeam() {
		team_id = NB_TEAMS+1;
		++NB_TEAMS;
	}
}
