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
    public override void enter()
    {
        Debug.Log("Enter SelectingWaterState");
        m_EState = EStates.SelectingWaterToPushState;

        Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
        CmdEnter(ray.origin, ray.direction);

        base.enter();
    }

    [Command]
    void CmdEnter(Vector3 _origin, Vector3 _direction)
    {
        m_waterReserve = getWaterReserve(_origin, _direction);
        RpcProvideWaterReserveToClient(m_waterReserve.GetComponent<NetworkIdentity>());
    }

    [ClientRpc]
    void RpcProvideWaterReserveToClient(NetworkIdentity _waterReserveNetId)
    {
        m_waterReserve = _waterReserveNetId.GetComponent<WaterReserve>();
    }

    [Client]
    public override void handleAction(EAction _action)
    {
        switch(_action)
        {
            case EAction.PushWater:
                m_character.m_currentActionState = m_character.m_statePool[(int)EStates.PushingWaterState];

                if (!m_waterReserve)
                    Debug.Break();
                GetComponent<Animator>().SetBool("SelectPush", true);
                CmdPushWater(m_waterReserve.GetComponent<NetworkIdentity>());
                (m_character.m_currentActionState as PushingWaterState).init(Vector3.zero, m_angle, false);

                m_character.m_currentActionState.enter();
                break;
        }

        base.handleAction(_action);
    }

    [Command]
    void CmdPushWater(NetworkIdentity _waterReserveNetId)
    {
        if (!_waterReserveNetId)
            return;

        m_character.m_waterGroup = Instantiate(Manager.getInstance().m_waterGroupPrefab).GetComponent<WaterGroup>();
		m_character.m_waterGroup.name = "" + m_character.GetComponent<NetworkIdentity>().netId; //= Character.count.ToString();

        WaterReserve waterReserve = _waterReserveNetId.GetComponent<WaterReserve>();

        m_character.m_waterGroup.initSelect(waterReserve.transform.position, waterReserve, m_minDropVolume, m_volumeWanted, m_speed);
        NetworkServer.Spawn(m_character.m_waterGroup.gameObject);

        RpcProvideWaterGroupToClient(m_character.m_waterGroup.GetComponent<NetworkIdentity>());
    }

    [ClientRpc]
    void RpcProvideWaterGroupToClient(NetworkIdentity _waterGroupNetId)
    {
        WaterGroup waterGroup = _waterGroupNetId.GetComponent<WaterGroup>();
        m_character.m_waterGroup = waterGroup;
    }

    [Client]
    public override void exit()
    {
        m_character.m_currentActionState = null;

        base.exit();
    }

    [Server]
    private WaterReserve getWaterReserve(Vector3 _origin, Vector3 _direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(_origin, _direction, out hit, m_distToSelect) && hit.collider.gameObject.layer == LayerMask.NameToLayer("Reserve"))
        {
            return hit.collider.GetComponent<WaterReserve>();
        }
        else
        {
            return getNearestWaterReserve();
        }
    }

    [Server]
    private WaterReserve getNearestWaterReserve()
    {
        Collider[] colList = Physics.OverlapSphere(m_character.transform.position, m_distToSelectAuto,
                                                   1 << LayerMask.NameToLayer("Reserve"), QueryTriggerInteraction.Collide);

        if (colList.Length < 1)
        {
            WaterReserve waterReserve = Instantiate(Manager.getInstance().m_waterReservePrefab).GetComponent<WaterReserve>();
            waterReserve.init(m_character.transform.position + m_character.transform.forward);
            NetworkServer.Spawn(waterReserve.gameObject);

            return waterReserve;
        }

        int nearestIndex = 0;
        float distNearest = Vector3.Distance(m_character.transform.position, colList[0].transform.position);
        for (int i = 1; i < colList.Length; ++i)
        {
            float dist = Vector3.Distance(m_character.transform.position, colList[i].transform.position);
            if (dist < distNearest)
            {
                nearestIndex = i;
                distNearest = dist;
            }
        }

        return colList[nearestIndex].GetComponent<WaterReserve>();
    }
}
