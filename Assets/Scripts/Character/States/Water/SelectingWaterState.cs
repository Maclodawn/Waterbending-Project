using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SelectingWaterState : AbleToFallState
{

    public float m_distToSelectAuto = 0.0f;
    public float m_distToSelect;

    public float m_minDropVolume = 0.25f;
    public float m_volumeWanted = 10.0f;
    public float m_speed = 10.0f;

    public float m_angle = 1f;

    WaterReserve m_waterReserve;

    [Client]
    public override void enter(Character _character)
    {
        Debug.Log("Enter SelectingWaterState");
        m_EState = EStates.SelectingWaterToPushState;

        Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
        CmdEnter(_character.GetComponent<NetworkIdentity>(), ray.origin, ray.direction);

        base.enter(_character);
    }

    [Command]
    void CmdEnter(NetworkIdentity _characterNetId, Vector3 _origin, Vector3 _direction)
    {
        m_waterReserve = getWaterReserve(_characterNetId.GetComponent<Character>(), _origin, _direction);
        RpcProvideWaterReserveToClient(m_waterReserve.GetComponent<NetworkIdentity>());
    }

    [ClientRpc]
    void RpcProvideWaterReserveToClient(NetworkIdentity _waterReserveNetId)
    {
        m_waterReserve = _waterReserveNetId.GetComponent<WaterReserve>();
    }

    [Client]
    public override void handleAction(Character _character, EAction _action)
    {
        switch(_action)
        {
            case EAction.PushWater:
                _character.m_currentActionState = _character.m_statePool[(int)EStates.PushingWaterState];

                if (!_character || !m_waterReserve)
                    Debug.Break();
                CmdPushWater(_character.GetComponent<NetworkIdentity>(), m_waterReserve.GetComponent<NetworkIdentity>());
                (_character.m_currentActionState as PushingWaterState).init(Vector3.zero, m_angle, false);

                _character.m_currentActionState.enter(_character);
                break;
        }

        base.handleAction(_character, _action);
    }

    [Command]
    void CmdPushWater(NetworkIdentity _characterNetId, NetworkIdentity _waterReserveNetId)
    {
        if (!_waterReserveNetId)
            return;

        Character character = _characterNetId.GetComponent<Character>();
        character.m_waterGroup = Instantiate(Manager.getInstance().m_waterGroupPrefab).GetComponent<WaterGroup>();
        character.m_waterGroup.name = Character.count.ToString();

        WaterReserve waterReserve = _waterReserveNetId.GetComponent<WaterReserve>();

        character.m_waterGroup.initSelect(waterReserve.transform.position, waterReserve, m_minDropVolume, m_volumeWanted, m_speed);
        NetworkServer.Spawn(character.m_waterGroup.gameObject);

        RpcProvideWaterGroupToClient(character.m_waterGroup.GetComponent<NetworkIdentity>());
    }

    [ClientRpc]
    void RpcProvideWaterGroupToClient(NetworkIdentity _waterGroupNetId)
    {
        Character character = GetComponent<Character>();
        WaterGroup waterGroup = _waterGroupNetId.GetComponent<WaterGroup>();
        character.m_waterGroup = waterGroup;
    }

    [Client]
    public override void exit(Character _character)
    {
        _character.m_currentActionState = null;

        base.exit(_character);
    }

    [Server]
    private WaterReserve getWaterReserve(Character _character, Vector3 _origin, Vector3 _direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(_origin, _direction, out hit, m_distToSelect) && hit.collider.gameObject.layer == LayerMask.NameToLayer("Reserve"))
        {
            return hit.collider.GetComponent<WaterReserve>();
        }
        else
        {
            return getNearestWaterReserve(_character);
        }
    }

    [Server]
    private WaterReserve getNearestWaterReserve(Character _character)
    {
        Collider[] colList = Physics.OverlapSphere(_character.transform.position, m_distToSelectAuto,
                                                   1 << LayerMask.NameToLayer("Reserve"), QueryTriggerInteraction.Collide);

        if (colList.Length < 1)
        {
            WaterReserve waterReserve = Instantiate(Manager.getInstance().m_waterReservePrefab).GetComponent<WaterReserve>();
            waterReserve.init(_character.transform.position + _character.transform.forward);
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
