using UnityEngine;
using System.Collections;

public class RunSprintingState : AbleToJumpState
{

    public override void enter(Character _character)
    {
        ////Debug.Log(">RunSprintingState");

        base.enter(_character);
    }

    public override void handleMovement(Character _character, EMovement _movement)
    {
        switch (_movement)
        {
            case EMovement.None:
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.StandingState];
                _character.m_currentMovementState.enter(_character);
                break;
        }
        base.handleMovement(_character, _movement);
    }

    public override void update(Character _character)
    {
        Vector2 dir = _character.m_inputDirection.normalized * _character.m_currentMoveSpeed;
        _character.m_velocity.x = dir.x;
        _character.m_velocity.z = dir.y;

        base.update(_character);
    }
}
