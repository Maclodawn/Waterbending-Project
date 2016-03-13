using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PullingWaterState : AbleToFallState
{
    private WaterReserve m_waterReserve;

    public GameObject m_waterGroupPrefab;
    public GameObject m_waterTargetPrefab;

    private GameObject m_target;
    public Vector3 m_targetOffset;

    public float m_minDropVolume = 0.25f;
    public float m_volumeWanted = 10.0f;
    public float m_speed = 10.0f;
    
    public float m_distToPull = 0.0f;
    int count = 0;

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

        CmdEnter(GetComponent<NetworkIdentity>());

        base.enter(_character);
    }

    [Command]
    void CmdEnter(NetworkIdentity _characterIdentity)
    {
        m_target = GameObject.Instantiate(m_waterTargetPrefab);
        Quaternion quaternion = Quaternion.FromToRotation(Vector3.forward, transform.forward);
        Vector3 vect = quaternion * m_targetOffset;

        Character character = _characterIdentity.GetComponent<Character>();
        m_waterReserve = getNearestWaterReserve(character);

        character.m_waterGroup = Instantiate<GameObject>(Manager.getInstance().m_waterGroupPrefab).GetComponent<WaterGroup>();
        character.m_waterGroup.name = count.ToString();

        character.m_waterGroup.initPull(m_waterReserve.transform.position, m_waterReserve, m_minDropVolume, m_volumeWanted, transform.position + vect, m_speed);

        NetworkServer.SpawnWithClientAuthority(m_target, gameObject);
        NetworkIdentity netId = m_target.GetComponent<NetworkIdentity>();
        RpcStart(netId);
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
        Collider[] colList = Physics.OverlapSphere(_character.transform.position, m_distToPull,
                                                   1 << LayerMask.NameToLayer("Reserve"), QueryTriggerInteraction.Collide);

        if (colList.Length < 1)
        {
            WaterReserve waterReserve = Instantiate<GameObject>(Manager.getInstance().m_waterReservePrefab).GetComponent<WaterReserve>();
            waterReserve.init(_character.transform.position + _character.transform.forward, 10);
            NetworkServer.Spawn(waterReserve.gameObject);
            return waterReserve;
        }

        int nearestIndex = 0;
        float distNearest = Vector3.Distance(_character.transform.position, colList[0].transform.position);
        for (int i = 1; i < colList.Length; ++i)
        {
            float dist = Vector3.Distance(_character.transform.position, colList[i].transform.position);
            if (dist < distNearest)
            {
                nearestIndex = i;
                distNearest = dist;
            }
        }

        return colList[nearestIndex].GetComponent<WaterReserve>();
    }
}
