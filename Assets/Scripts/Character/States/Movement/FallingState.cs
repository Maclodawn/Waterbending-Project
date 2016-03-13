using UnityEngine;
using System.Collections;

public class FallingState : CharacterState
{

    public override void enter(Character _character)
    {
        Debug.Log("Enter FallingState");
        m_EState = EStates.FallingState;
        _character.m_animator.SetBool("Fall", true);
        _character.m_controller.height = 0;

        base.enter(_character);
    }

    public override void handleMovement(Character _character, EMovement _movement)
    {
        switch (_movement)
        {
            case EMovement.Stabilize:
                _character.m_currentMovementState.exit(_character);
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.JumpDescendingState];
                _character.m_currentMovementState.enter(_character);
                break;
        }
    }

    public override void fixedUpdate(Character _character)
    {
        _character.m_velocity += _character.m_gravity;
    }

    public override void update(Character _character)
    {
        if (Physics.Raycast(transform.position + Vector3.up, -Vector3.up, 1.0f))
        {
            _character.m_currentMovementState.exit(_character);
            _character.m_currentMovementState = _character.m_statePool[(int)EStates.FallenState];
            _character.m_currentMovementState.enter(_character);
        }

        base.update(_character);
    }

    public override void exit(Character _character)
    {
        _character.m_animator.SetBool("Fall", false);
        _character.m_controller.height = 1.8f;

        base.exit(_character);
    }
}
