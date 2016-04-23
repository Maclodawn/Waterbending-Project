using UnityEngine;
using System.Collections;

public class AbleToJumpState : AbleToFallState
{
    public bool m_gettingUp = false;

    public override void handleAction(Character _character, EAction _action)
    {
        if (_character.m_currentActionState == null)
        {
            switch (_action)
            {
                case EAction.PullWater:
                    _character.m_currentActionState = _character.m_statePool[(int)EStates.PullingWaterState];
                    _character.m_currentActionState.enter(_character);
                    break;
                case EAction.SelectWaterToPush:
                    _character.m_currentActionState = _character.m_statePool[(int)EStates.SelectingWaterToPushState];
                    _character.m_currentActionState.enter(_character);
                    break;
            }
        }

        base.handleAction(_character, _action);
    }

    public override void handleMovement(Character _character, EMovement _movement)
    {
        // We do not want to be able to change the movement to Jump if any action is not over
        if (_character.m_currentActionState != null)
        {
            return;
        }

        switch (_movement)
        {
            case EMovement.Jump:
                _character.m_currentMovementState.exit(_character);
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.JumpingState];
                _character.m_currentMovementState.enter(_character);
                break;
            case EMovement.Dodge:
                _character.m_currentMovementState.exit(_character);
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.DodgingState];
                _character.m_currentMovementState.enter(_character);
                break;
            case EMovement.Die:
                _character.m_currentMovementState.exit(_character);
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.DyingState];
                print(_character.m_statePool.Count + " " + (int)EStates.DyingState);
                _character.m_currentMovementState.enter(_character);
                break;
        }

        base.handleMovement(_character, _movement);
    }

    protected void initFixedUpdate(Character _character)
    {
        _character.m_velocity.x = 0;
        _character.m_velocity.z = 0;
    }

    public override void update(Character _character)
    {
        if (!m_gettingUp && !Physics.Raycast(transform.position, -Vector3.up, 0.1f))
        {
            if (_character.m_currentActionState)
                _character.m_currentActionState.exit(_character);
            _character.m_currentMovementState.exit(_character);
            _character.m_currentMovementState = _character.m_statePool[(int)EStates.JumpDescendingState];
            _character.m_currentMovementState.enter(_character);
        }

        base.update(_character);
    }
}
