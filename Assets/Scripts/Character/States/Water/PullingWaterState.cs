using UnityEngine;
using System.Collections;

public class PullingWaterState : AbleToFallState
{
    private WaterFlow m_waterFlow;

    // Relative to the character position
    public Vector3 m_waterPositionWanted;

    public override void enter(Character _character)
    {
        Debug.Log("Enter PullingWaterState");
        m_EState = EStates.PullingWaterState;

        _character.m_selectedReserve = _character.getNearestReserve();
        Transform waterflow = Instantiate(_character.m_waterFlowPrefab);
        m_waterFlow = waterflow.GetComponent<WaterFlow>();
        m_waterFlow.init(_character.m_selectedReserve,
                         _character.transform.position + m_waterPositionWanted,
                         10.0f);

        base.enter(_character);
    }

    public override void handleAction(Character _character, EAction _action)
    {
        switch(_action)
        {
            case EAction.ReleaseWaterControl:
                _character.m_currentActionState.cancel(_character);
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
        Vector3 vect = quaternion * m_waterPositionWanted;
        m_waterFlow.updateTarget(_character.transform.position + vect);

        base.update(_character);
    }

    public override void cancel(Character _character)
    {
        m_waterFlow.releaseControl();
        _character.m_currentActionState = null;

        base.cancel(_character);
    }
}
