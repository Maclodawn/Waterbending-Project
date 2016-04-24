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
    public override void enter()
    {
        Debug.Log("Enter PullingWaterState");
        GetComponent<Animator>().SetBool("Pull", true);
        m_EState = EStates.PullingWaterState;


        CmdEnter();

        base.enter();
    }

    [Command]
    void CmdEnter()
    {
        m_waterReserve = getNearestWaterReserve();

        m_character.m_waterGroup = Instantiate(Manager.getInstance().m_waterGroupPrefab).GetComponent<WaterGroup>();
        m_character.m_waterGroup.name = Character.count.ToString();


        GameObject target = GameObject.Instantiate(Manager.getInstance().m_waterTargetPrefab);
        Quaternion quaternion = Quaternion.FromToRotation(Vector3.forward, transform.forward);
        Vector3 vect = quaternion * m_targetOffset;
        target.transform.position = transform.position + vect;
        NetworkServer.SpawnWithClientAuthority(target, gameObject);

        m_character.m_waterGroup.initPull(m_waterReserve.transform.position, m_waterReserve, m_minDropVolume, m_volumeWanted, target, m_speed);
        NetworkServer.Spawn(m_character.m_waterGroup.gameObject);

        RpcStart(m_character.m_waterGroup.GetComponent<NetworkIdentity>(), m_character.m_waterGroup.m_target.GetComponent<NetworkIdentity>());
    }

    [Client]
    public override void handleAction(EAction _action)
    {
        switch (_action)
        {
            case EAction.ReleaseWaterControl:
                cancel();
                Debug.Log("ReleaseWaterControl");
                GetComponent<Animator>().SetBool("Pull", false);
                break;
            case EAction.TurnWaterAround:
                m_character.m_currentActionState = m_character.m_statePool[(int)EStates.TurningWaterAroundState];
                m_character.m_currentActionState.enter();
                break;
        }

        base.handleAction(_action);
    }

    [Client]
    public override void update()
    {
        if (m_character.m_waterGroup && m_character.m_waterGroup.m_target)
        {
            Quaternion quaternion = Quaternion.FromToRotation(Vector3.forward, transform.forward);
            Vector3 vect = quaternion * m_targetOffset;
            if (m_character.m_waterGroup)
            {
                m_character.m_waterGroup.m_target.transform.position = transform.position + vect;
            }
            else if (m_character.m_currentActionState)
            {
                m_character.m_currentActionState.exit();
            }
        }

        base.update();
    }

    [Client]
    public override void exit()
    {
        cancel();


        base.exit();
    }

    [Client]
    private void cancel()
    {
        CmdCancel();
        m_character.m_currentActionState = null;
    }

    [Command]
    private void CmdCancel()
    {
        GetComponent<Character>().m_waterGroup.releaseControl();
    }

    [Server]
    private WaterReserve getNearestWaterReserve()
    {
        Collider[] colList = Physics.OverlapSphere(m_character.transform.position, m_distToPull,
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
