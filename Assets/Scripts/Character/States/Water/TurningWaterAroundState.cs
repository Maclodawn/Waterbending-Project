using UnityEngine;
using System.Collections;

public class TurningWaterAroundState : AbleToFallState
{
    public float m_radiusToTurnAround = 1;

    public override void enter(Character _character)
    {
        Debug.Log("Enter TurningWaterAroundState");
        m_EState = EStates.TurningWaterAroundState;

        _character.m_waterGroup.m_target.transform.position = _character.transform.position + _character.m_controller.center;
        _character.m_waterGroup.turnAround(m_radiusToTurnAround);
        
        base.enter(_character);
    }

    public override void handleAction(Character _character, EAction _action)
    {
        switch(_action)
        {
            case EAction.PushWater:
                _character.m_currentActionState = _character.m_statePool[(int)EStates.PushingWaterState];
                _character.m_currentActionState.enter(_character);
                break;
        }

        base.handleAction(_character, _action);
    }
}
