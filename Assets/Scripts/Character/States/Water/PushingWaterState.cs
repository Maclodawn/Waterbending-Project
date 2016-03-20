using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PushingWaterState : AbleToFallState
{

    public float m_speed;
    Vector3 m_offsetToFling;
    float m_alpha;
    bool m_fromTurn = false;
    [SyncVar]
    bool isReady = false;

    float time = 0.0f;
    public float cooldown;

    [Client]
    public void init(Vector3 _offsetToFling, float _alpha, bool _fromTurn)
    {
        time = 0.0f;
        m_offsetToFling = _offsetToFling;
        m_alpha = _alpha;
        m_fromTurn = _fromTurn;
        CmdInit(m_offsetToFling, m_alpha, m_fromTurn, m_speed);
    }

    [Command]
    void CmdInit(Vector3 _offsetToFling, float _alpha, bool _fromTurn, float _speed)
    {
        m_offsetToFling = _offsetToFling;
        m_alpha = _alpha;
        m_fromTurn = _fromTurn;
        m_speed = _speed;
    }

    [Client]
    public override void enter(Character _character)
    {
        Debug.Log("Enter PushingWaterState");
        m_EState = EStates.PushingWaterState;

        GameObject newTarget = AutoAim();
        if (newTarget)
        {
            CmdEnter(_character.GetComponent<NetworkIdentity>(), newTarget.GetComponent<NetworkIdentity>());
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
            CmdEnter(_character.GetComponent<NetworkIdentity>(), ray.origin, ray.direction);
        }

        base.enter(_character);
    }

    [Command]
    void CmdEnter(NetworkIdentity _characterIdentity, NetworkIdentity _targetIdentity)
    {
        GameObject newTarget = _targetIdentity.gameObject;

        enter(_characterIdentity.getComponent<Character>(), newTarget);
    }

    [Command]
    void CmdEnter(NetworkIdentity _characterIdentity, Vector3 _origin, Vector3 _direction)
    {
        GameObject newTarget = GameObject.Instantiate(Manager.getInstance().m_waterTargetPrefab);
        newTarget.transform.position = _origin + _direction * 40.0f;
        NetworkServer.SpawnWithClientAuthority(newTarget, gameObject);

        enter(_characterIdentity.getComponent<Character>(), newTarget);
    }

    [Server]
    void enter(Character _character, GameObject newTarget)
    {
        _character.m_waterGroup.setTarget(newTarget);
        if (m_fromTurn)
            _character.m_waterGroup.flingFromTurn(m_speed, _character.transform.position + _character.m_controller.center + m_offsetToFling, m_alpha);
        else
            _character.m_waterGroup.flingFromSelect(m_speed, _character.transform.position + _character.m_controller.center + m_offsetToFling, m_alpha);
    }

    [Client]
    public override void update(Character _character)
    {
        if (!isReady)
            return;

        if (!_character.m_waterGroup.m_flingingFromSelect && !_character.m_waterGroup.m_flingingFromTurn && time > cooldown)
        {
            _character.m_currentActionState = null;
        }

        time += Time.deltaTime;
        base.update(_character);
    }

    public override void exit(Character _character)
    {
        _character.m_currentActionState = null;

        base.exit(_character);
    }

    [Client]
    GameObject AutoAim()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closestplayer = null;
        float closestPlayerDistance = float.MaxValue;

        foreach (GameObject player in players)
        {
            if (player == gameObject)
                continue;

            Vector3 point = Camera.main.WorldToViewportPoint(player.transform.position);
            if (point.z < 0 || point.x < 0 || point.x > 1 || point.y < 0 || point.y > 1)
                continue;

            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (closestPlayerDistance > dist)
            {
                closestplayer = player;
                closestPlayerDistance = dist;
            }
        }

        return closestplayer;
    }
}
