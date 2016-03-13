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

        Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
        CmdEnter(_character.GetComponent<NetworkIdentity>(), ray.origin, ray.direction);

        base.enter(_character);
    }

    [Command]
    void CmdEnter(NetworkIdentity _characterIdentity, Vector3 _origin, Vector3 _direction)
    {
        isReady = true;
        RaycastHit hit;
        GameObject newTarget;
        if (Physics.Raycast(_origin, _direction, out hit, 1000.0f))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                newTarget = hit.collider.gameObject;
            }
            else
            {
                newTarget = GameObject.Instantiate(Manager.getInstance().m_waterTargetPrefab);
                newTarget.transform.position = hit.point;
                NetworkServer.SpawnWithClientAuthority(newTarget, gameObject);
            }
        }
        else
        {
            newTarget = GameObject.Instantiate(Manager.getInstance().m_waterTargetPrefab);
            newTarget.transform.position = _origin + _direction * 1000.0f;
            NetworkServer.SpawnWithClientAuthority(newTarget, gameObject);
        }

        Character character = _characterIdentity.GetComponent<Character>();
        character.m_waterGroup.setTarget(newTarget);
        if (m_fromTurn)
            character.m_waterGroup.flingFromTurn(m_speed, character.transform.position + character.m_controller.center + m_offsetToFling, m_alpha);
        else
            character.m_waterGroup.flingFromSelect(m_speed, character.transform.position + character.m_controller.center + m_offsetToFling, m_alpha);
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
}
