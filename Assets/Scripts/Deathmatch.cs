using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

public class Deathmatch : NetworkBehaviour
{

    private InformationsLog informations = null;
    private static LinkedList<HealthComponent> players_alive = null;
    private static LinkedList<HealthComponent> players_dead = null;
    private HealthComponent my_player = null;
    private int ID = -1;
    private static int NB_INSTANCES = 0;
    private bool end = false;

    //Initializations
    public void Start()
    {
        NB_INSTANCES++;
        ID = NB_INSTANCES;

        //if those are null, we instanciate both lists and fill them up
        if (players_alive == null || players_dead == null)
        {
            players_alive = new LinkedList<HealthComponent>();
            players_dead = new LinkedList<HealthComponent>();

            //Retrieves all players
            GameObject[] go_players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject tmp_go_player in go_players)
            {
                //we're only interested in their healths
                HealthComponent tmp_health = tmp_go_player.GetComponent<HealthComponent>();

                //throwing exception because it is not normal
                if (tmp_health == null)
                    throw new Exception("HealthComponent missing in player.");

                players_alive.AddFirst(tmp_health);
            }
        }

        //Retrieving my own player
        my_player = gameObject.GetComponentInParent<HealthComponent>();
        if (my_player == null)
            throw new ArgumentException("HealthComponent missing in player.");

        //retrieving InformationsLog
        informations = GameObject.Find("InformationsLog").GetComponent<InformationsLog>();

        //setting team
        if (hasAuthority && !GetComponent<FakePlayer>())
        {
            Teamate teamate = GetComponent<Teamate>();
            MyTeamId myTeamId = FindObjectOfType<MyTeamId>();
            //teamate.addToTeam(myTeamId.hideAndGetTeamId());
            CmdSetTeam(myTeamId.hideAndGetTeamId());
        }
    }

    [Command]
    private void CmdSetTeam(int _team_id)
    {
        Teamate teamate = GetComponent<Teamate>();
        teamate.addToTeam(_team_id);
        RpcSetTeamClient(_team_id);
    }

    [ClientRpc]
    private void RpcSetTeamClient(int _team_id)
    {
        print(_team_id);
        Teamate teamate = GetComponent<Teamate>();
        teamate.addToTeam(_team_id);
    }

    //At each frame, checks if a new player is dead...
    //We assume we can only die once!
    public void Update()
    {
        //victory detection
        if (!end && players_alive.Count < 2 && players_alive.Contains(my_player))
        {
            informations.log(gameObject.name + " WINS!");
            end = true;
        }

        //find dead players in players_alive and add them to players_dead
        foreach (HealthComponent tmp_player in players_alive)
        {
            if (!isAlive(tmp_player))
                players_dead.AddFirst(tmp_player);
        }

        //then remove them from players_alive
        foreach (HealthComponent tmp_player in players_dead)
        {
            if (players_alive.Contains(tmp_player))
                players_alive.Remove(tmp_player);
            else break; //We assume we go through the list in order and newly dead players are upfront;
            //therefore we don't need to check the rest of the list 
        }
    }

    private bool isAlive(HealthComponent player_health)
    {
        return player_health.Health > 0.1f;
    }
}
