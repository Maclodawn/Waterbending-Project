﻿using UnityEngine;
using System.Collections;

public class PullingWaterState : AbleToFallState
{
    public GameObject m_waterReservePrefab;
    private WaterReserve m_waterReserve;

    public GameObject m_waterProjectilePrefab;
    private WaterProjectile m_waterProjectile;

    private GameObject m_target;

    void Start()
    {
        m_target = new GameObject();
        m_target.name = "WaterTarget";
    }

    public override void enter(Character _character)
    {
        Debug.Log("Enter PullingWaterState");
        m_EState = EStates.PullingWaterState;

        m_waterReserve = getNearestWaterReserve(_character);
        m_waterProjectile = Instantiate<GameObject>(m_waterProjectilePrefab).GetComponent<WaterProjectile>();
        m_target.transform.position = m_waterReserve.transform.position + Vector3.up * 2;
        m_waterProjectile.transform.position = m_waterReserve.transform.position;
        m_waterProjectile.init(m_waterReserve, 10, m_target, 10);

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

    private void cancel(Character _character)
    {
        m_waterProjectile.releaseControl();
        _character.m_currentActionState = null;
    }

    private WaterReserve getNearestWaterReserve(Character _character)
    {
        WaterReserve waterReserve = Instantiate<GameObject>(m_waterReservePrefab).GetComponent<WaterReserve>();
        waterReserve.setVolume(10);
        waterReserve.transform.position = _character.transform.position + _character.transform.forward;
        return waterReserve;
    }
}
