using UnityEngine;
using System.Collections;

public class AbleToJumpState : AbleToFallState
{
    public bool m_gettingUp = false;

    public override void handleAction(EAction _action)
    {
        if (_action == EAction.Guard && !(m_character.m_currentActionState is GuardingState))
        {
            if (m_character.m_currentActionState != null)
                m_character.m_currentActionState.exit();
            m_character.m_currentActionState = m_character.m_statePool[(int)EStates.CounteringWaterState];
            m_character.m_currentActionState.enter();
        }
        else if (m_character.m_currentActionState == null)
        {
            switch (_action)
            {
                case EAction.PullWater:
                    m_character.m_currentActionState = m_character.m_statePool[(int)EStates.PullingWaterState];
                    m_character.m_currentActionState.enter();
                    break;
                case EAction.SelectWaterToPush:
                    m_character.m_currentActionState = m_character.m_statePool[(int)EStates.SelectingWaterToPushState];
                    m_character.m_currentActionState.enter();
                    break;
            }
        }

        base.handleAction(_action);
    }

    public override void handleMovement(EMovement _movement)
    {
        // We do not want to be able to change the movement to Jump if any action is not over
        if (m_character.m_currentActionState != null)
        {
            return;
        }

        switch (_movement)
        {
            case EMovement.Jump:
                m_character.m_currentMovementState.exit();
                m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.JumpingState];
                m_character.m_currentMovementState.enter();
                break;
            case EMovement.Dodge:
                m_character.m_currentMovementState.exit();
                m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.DodgingState];
                m_character.m_currentMovementState.enter();
                break;
        }

        base.handleMovement(_movement);
    }

    protected void initFixedUpdate()
    {
        m_character.m_velocity.x = 0;
        m_character.m_velocity.z = 0;
    }

    public override void update()
    {
        if (!m_gettingUp && !Physics.Raycast(transform.position, -Vector3.up, 0.1f))
        {
            if (m_character.m_currentActionState)
                m_character.m_currentActionState.exit();
            m_character.m_currentMovementState.exit();
            m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.JumpDescendingState];
            m_character.m_currentMovementState.enter();
        }

        base.update();
    }
}
