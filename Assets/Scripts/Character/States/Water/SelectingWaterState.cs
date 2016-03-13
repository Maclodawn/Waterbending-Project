using UnityEngine;
using System.Collections;

public class SelectingWaterState : AbleToFallState
{

    public float m_distToSelectAuto = 0.0f;
    public float m_distToSelect;

    public float m_minDropVolume = 0.25f;
    public float m_volumeWanted = 10.0f;
    public float m_speed = 10.0f;

    WaterReserve m_waterReserve;

    public override void enter(Character _character)
    {
        Debug.Log("Enter SelectingWaterState");
        m_EState = EStates.SelectingWaterToPushState;

        m_waterReserve = getWaterReserve(_character);

        base.enter(_character);
    }

    public override void handleAction(Character _character, EAction _action)
    {
        switch(_action)
        {
            case EAction.PushWater:
                _character.m_currentActionState = _character.m_statePool[(int)EStates.PushingWaterState];

                _character.m_waterGroup = Instantiate<GameObject>(Manager.getInstance().m_waterGroupPrefab).GetComponent<WaterGroup>();
                _character.m_waterGroup.initSelect(m_waterReserve.transform.position, m_waterReserve, m_minDropVolume, m_volumeWanted, m_speed);
                (_character.m_currentActionState as PushingWaterState).init(Vector3.zero, 1f, false);

                _character.m_currentActionState.enter(_character);
                break;
        }

        base.handleAction(_character, _action);
    }

    private WaterReserve getWaterReserve(Character _character)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, m_distToSelect) && hit.collider.gameObject.layer == LayerMask.NameToLayer("Reserve"))
        {
            return hit.collider.GetComponent<WaterReserve>();
        }
        else
        {
            return getNearestWaterReserve(_character);
        }
    }



    private WaterReserve getNearestWaterReserve(Character _character)
    {
        Collider[] colList = Physics.OverlapSphere(_character.transform.position, m_distToSelectAuto,
                                                   1 << LayerMask.NameToLayer("Reserve"), QueryTriggerInteraction.Collide);
        
        if (colList.Length < 1)
        {
            WaterReserve waterReserve = Instantiate<GameObject>(Manager.getInstance().m_waterReservePrefab).GetComponent<WaterReserve>();
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
