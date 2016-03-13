using UnityEngine;
using System.Collections;

public class PullingWaterState : AbleToFallState
{
    private WaterReserve m_waterReserve;

    public Vector3 m_targetOffset;

    public float m_minDropVolume = 0.25f;
    public float m_volumeWanted = 10.0f;
    public float m_speed = 10.0f;

    public float m_distToPull = 0.0f;
    int count = 0;

    public override void enter(Character _character)
    {
        Debug.Log("Enter PullingWaterState");
        m_EState = EStates.PullingWaterState;

        m_waterReserve = getNearestWaterReserve(_character);

        _character.m_waterGroup = Instantiate<GameObject>(Manager.getManager().m_waterGroupPrefab).GetComponent<WaterGroup>();
        _character.m_waterGroup.name = count.ToString();

        Quaternion quaternion = Quaternion.FromToRotation(Vector3.forward, transform.forward);
        Vector3 vect = quaternion * m_targetOffset;
        _character.m_waterGroup.initPull(m_waterReserve.transform.position, m_waterReserve, m_minDropVolume, m_volumeWanted, transform.position + vect, m_speed);

        base.enter(_character);
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
        Quaternion quaternion = Quaternion.FromToRotation(Vector3.forward, transform.forward);
        Vector3 vect = quaternion * m_targetOffset;
        _character.m_waterGroup.m_target.transform.position = transform.position + vect;
        base.update(_character);
    }

    private void cancel(Character _character)
    {
        _character.m_waterGroup.releaseControl();
        _character.m_currentActionState = null;
    }

    private WaterReserve getNearestWaterReserve(Character _character)
    {
        Collider[] colList = Physics.OverlapSphere(_character.transform.position, m_distToPull,
                                                   1 << LayerMask.NameToLayer("Reserve"), QueryTriggerInteraction.Collide);

        if (colList.Length < 1)
        {
            WaterReserve waterReserve = Instantiate<GameObject>(Manager.getManager().m_waterReservePrefab).GetComponent<WaterReserve>();
            waterReserve.setVolume(10);
            waterReserve.transform.position = _character.transform.position + _character.transform.forward;
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
