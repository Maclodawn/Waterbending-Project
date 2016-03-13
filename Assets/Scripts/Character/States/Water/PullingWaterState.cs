using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PullingWaterState : AbleToFallState
{
    private WaterReserve m_waterReserve;

    public Vector3 m_targetOffset;

    public float m_minDropVolume = 0.25f;
    public float m_volumeWanted = 10.0f;
    public float m_speed = 10.0f;
    
    public float m_distToPull = 0.0f;

    [ClientRpc]
    void RpcStart(NetworkIdentity _waterGroupNetId, NetworkIdentity _targetNetId)
    {
        Character character = GetComponent<Character>();
        character.m_waterGroup = _waterGroupNetId.GetComponent<WaterGroup>();
        character.m_waterGroup.setTarget(_targetNetId.gameObject);
        character.m_waterGroup.m_target.GetComponent<TargetSync>().CmdSetReady();
    }

    [Client]
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
        Character character = _characterIdentity.GetComponent<Character>();
        
        m_waterReserve = getNearestWaterReserve(character);

        character.m_waterGroup = Instantiate(Manager.getInstance().m_waterGroupPrefab).GetComponent<WaterGroup>();
        character.m_waterGroup.name = Character.count.ToString();


        GameObject target = GameObject.Instantiate(Manager.getInstance().m_waterTargetPrefab);
        Quaternion quaternion = Quaternion.FromToRotation(Vector3.forward, transform.forward);
        Vector3 vect = quaternion * m_targetOffset;
        target.transform.position = transform.position + vect;
        NetworkServer.SpawnWithClientAuthority(target, gameObject);

        character.m_waterGroup.initPull(m_waterReserve.transform.position, m_waterReserve, m_minDropVolume, m_volumeWanted, target, m_speed);
        NetworkServer.Spawn(character.m_waterGroup.gameObject);

        RpcStart(character.m_waterGroup.GetComponent<NetworkIdentity>(), character.m_waterGroup.m_target.GetComponent<NetworkIdentity>());
    }

    [Client]
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

    [Client]
    public override void update(Character _character)
    {
        if (_character.m_waterGroup && _character.m_waterGroup.m_target)
        {
            Quaternion quaternion = Quaternion.FromToRotation(Vector3.forward, transform.forward);
            Vector3 vect = quaternion * m_targetOffset;
            _character.m_waterGroup.m_target.transform.position = transform.position + vect;
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
            WaterReserve waterReserve = Instantiate(Manager.getInstance().m_waterReservePrefab).GetComponent<WaterReserve>();
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
