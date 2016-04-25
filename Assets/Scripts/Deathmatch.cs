using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

public class Deathmatch : NetworkBehaviour
{

    private static LinkedList<HealthComponent> m_playersAlive = null;
    private static LinkedList<HealthComponent> m_playersDead = null;
    
    private HealthComponent m_healthComponent = null;
    private Teammate m_teammate;

    //Initializations
    public void Start()
    {
        m_teammate = GetComponent<Teammate>();

        if (!NetworkClient.active)
            return;

        //if those are null, we instantiate both lists and fill them up
        if (m_playersAlive == null || m_playersDead == null)
        {
            m_playersAlive = new LinkedList<HealthComponent>();
            m_playersDead = new LinkedList<HealthComponent>();

            //Retrieves all players
            GameObject[] go_players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject tmp_go_player in go_players)
            {
                //we're only interested in their healths
                HealthComponent tmp_health = tmp_go_player.GetComponent<HealthComponent>();

                //throwing exception because it is not normal
                if (tmp_health == null)
                    throw new Exception("HealthComponent missing in player.");

                m_playersAlive.AddFirst(tmp_health);
            }
        }

        //Retrieving my own player
        m_healthComponent = gameObject.GetComponentInParent<HealthComponent>();
        if (m_healthComponent == null)
            throw new ArgumentException("HealthComponent missing in player.");

        //setting team
        if (hasAuthority && !GetComponent<FakePlayer>())
        {
            MyTeamId myTeamId = FindObjectOfType<MyTeamId>();
            CmdSetTeam(myTeamId.hideAndGetTeamId());
        }
    }

    [Command]
    private void CmdSetTeam(int _teamId)
    {
        m_teammate.addToTeam(_teamId);
        RpcSetTeamClient(_teamId);
    }

    [ClientRpc]
    private void RpcSetTeamClient(int _teamId)
    {
        m_teammate.addToTeam(_teamId);
    }

    //At each frame, checks if a new player is dead...
    //We assume we can only die once!
    [ClientCallback]
    public void Update()
    {
        if (!NetworkClient.active)
            return;

        //victory detection: adding team testing
        bool onlyMyTeamAlive = true;

        foreach (HealthComponent player in m_playersAlive)
        {
            if (player)
            {
                Teammate playerTeammate = player.GetComponent<Teammate>();
                if (m_teammate && playerTeammate && m_teammate.m_teamId != playerTeammate.m_teamId)
                {
                    onlyMyTeamAlive = false;
                    break;
                }
            }
        }

        if (onlyMyTeamAlive && m_playersAlive.Contains(m_healthComponent))
        {
            Manager.getInstance().m_winnerMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Text winningText = GameObject.Find("Winning_Text").GetComponent<Text>();
            winningText.text = "TEAM " + m_teammate.m_teamId + " WINS!";
        }
        else if (!onlyMyTeamAlive)
        {
            Manager.getInstance().m_winnerMenu.SetActive(false);
            if (!Manager.getInstance().isGamePaused())
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        //find dead players in players_alive and add them to players_dead
        foreach (HealthComponent player in m_playersAlive)
        {
            if (!isAlive(player))
                m_playersDead.AddFirst(player);
        }

        //then remove them from players_alive
        foreach (HealthComponent tmp_player in m_playersDead)
        {
            if (m_playersAlive.Contains(tmp_player))
                m_playersAlive.Remove(tmp_player);
            else break; //We assume we go through the list in order and newly dead players are upfront;
            //therefore we don't need to check the rest of the list 
        }
    }

    [Client]
    private bool isAlive(HealthComponent player_health)
    {
        return player_health.Health > 0.0f;
    }
}
