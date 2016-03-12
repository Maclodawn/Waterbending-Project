using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PullingWaterState : AbleToFallState
{
    public GameObject m_waterReservePrefab;
    private WaterReserve m_waterReserve;

    public GameObject m_waterGroupPrefab;
    public GameObject m_waterTargetPrefab;

    private GameObject m_target;
    public Vector3 m_targetOffset;

    public float m_minDropVolume = 0.25f;
    public float m_volumeWanted = 10.0f;
    public float m_speed = 10.0f;

    void Start()
    {
//         m_target = new GameObject();
//         m_target.AddComponent<TargetSync>();
//         m_target.name = "WaterTarget";
//         if (isLocalPlayer)
//         {
//             CmdStart();
//         }
    }

//     [Command]
//     void CmdStart()
//     {
//     }

    [ClientRpc]
    void RpcStart(NetworkIdentity _targetNetId)
    {
        m_target = _targetNetId.gameObject;
        m_target.GetComponent<TargetSync>().CmdSetReady();
    }

    public override void enter(Character _character)
    {
        Debug.Log("Enter PullingWaterState");
        m_EState = EStates.PullingWaterState;


        // Other Attributes networked automatically?
        CmdEnter(GetComponent<NetworkIdentity>());


        base.enter(_character);
    }

    [Command]
    void CmdEnter(NetworkIdentity _characterIdentity)
    {
        m_target = GameObject.Instantiate(m_waterTargetPrefab);
        Quaternion quaternion = Quaternion.FromToRotation(Vector3.forward, transform.forward);
        Vector3 vect = quaternion * m_targetOffset;
        m_target.transform.position = transform.position + vect;

        NetworkServer.SpawnWithClientAuthority(m_target, gameObject);
        NetworkIdentity netId = m_target.GetComponent<NetworkIdentity>();
        RpcStart(netId);
        //--
        Character character = _characterIdentity.GetComponent<Character>();
        m_waterReserve = getNearestWaterReserve(character);
        character.m_waterGroup = Instantiate<GameObject>(m_waterGroupPrefab).GetComponent<WaterGroup>();
        character.m_waterGroup.transform.position = m_waterReserve.transform.position;
        character.m_waterGroup.init(m_waterReserve, m_minDropVolume, m_volumeWanted, m_target, m_speed);
    }

    public override void handleAction(Character _character, EAction _action)
    {
        switch (_action)
        {
            case EAction.ReleaseWaterControl:
                cancel(_character);
                Debug.Log("ReleaseWaterControl");
                break;
            case EAction.TurnWaterAround:
                _character.m_currentActionState = _character.m_statePool[(int)EStates.TurningWaterAroundState];
                ((TurningWaterAroundState)_character.m_currentActionState).m_target = m_target;
                _character.m_currentActionState.enter(_character);
                break;
        }

        base.handleAction(_character, _action);
    }

    public override void update(Character _character)
    {
        if (m_target)
        {
            Quaternion quaternion = Quaternion.FromToRotation(Vector3.forward, transform.forward);
            Vector3 vect = quaternion * m_targetOffset;
            m_target.transform.position = transform.position + vect;
        }
        base.update(_character);
    }

    [Client]
    private void cancel(Character _character)
    {
        CmdCancel();
        _character.m_currentActionState = null;
    }

    [Command]
    private void CmdCancel()
    {
        GetComponent<Character>().m_waterGroup.releaseControl();
    }

    [Server]
    private WaterReserve getNearestWaterReserve(Character _character)
    {
        WaterReserve waterReserve = Instantiate<GameObject>(m_waterReservePrefab).GetComponent<WaterReserve>();
        waterReserve.init(_character.transform.position + _character.transform.forward, 10);
        NetworkServer.Spawn(waterReserve.gameObject);
        return waterReserve;
    }
}
