using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class Teammate : NetworkBehaviour
{

    private static int NB_TEAMS = 1;
    public int m_teamId = -1; //starts at 1, 0 == fakeplayers
    [SyncVar]
    int syncTeamId = -1;
    public InformationsLog infos = null;

    //private static string[] colors = {"red", "yellow", "blue", "magenta", "orange", "pink", "white"};

    public void Start()
    {
        //retrieving InformationsLog
        infos = GameObject.Find("InformationsLog").GetComponent<InformationsLog>();
    }

    void Update()
    {
        if (hasAuthority)
        {
            if (NetworkClient.active)
                CmdUpdateTeamId(m_teamId, GetComponent<NetworkIdentity>());
        }
        else
        {
            m_teamId = syncTeamId;
        }
    }

    [Command]
    void CmdUpdateTeamId(int _teamId, NetworkIdentity characterIdentity)
    {
        if (characterIdentity.netId == GetComponent<NetworkIdentity>().netId)
            syncTeamId = _teamId;
    }

    public bool isFriend(Teammate _teamate)
    {
        if (!_teamate)
            return false;

        return _teamate.m_teamId == m_teamId;
    }

    public void addToTeam(int _new_team_id)
    {
        if (_new_team_id > 6)
            m_teamId = 6;
        else
            m_teamId = _new_team_id;

        try
        {
            if (infos == null)
                infos = GameObject.Find("InformationsLog").GetComponent<InformationsLog>();

            infos.log("<b><color=\"yellow\">" + gameObject.name + "</color></b>: now joining team <b><color=\"yellow\">#" + m_teamId + "</color></b>");
        }
        catch (Exception)
        {
            Debug.LogError("Ununderstandable." + (infos == null) + gameObject);
        }
    }

    public void addToNewTeam()
    {
        m_teamId = NB_TEAMS + 1;
        ++NB_TEAMS;

        infos.log("<b><color=\"yellow\">" + gameObject.name + "</color></b>: now joining team <b><color=\"yellow\">#" + m_teamId + "</color></b>");
    }
}
