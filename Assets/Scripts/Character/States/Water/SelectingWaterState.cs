using UnityEngine;
using System.Collections;

public class SelectingWaterState : CharacterState
{

    public override void enter(Character _character)
    {
        Debug.Log("Enter SelectingWaterState");
        m_EState = EStates.SelectingWaterToPushState;

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
