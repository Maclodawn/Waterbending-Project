using UnityEngine;
using System.Collections;

public class JumpDescendingState : AbleToFallState
{

    public override void enter(Character _character)
    {
        Debug.Log("Enter JumpDescendingState");
        m_EState = EStates.JumpDescendingState;

        base.enter(_character);
    }

    public override void fixedUpdate(Character _character)
    {
        //COMPUTE PHYSIC TO JUMP-DESCEND
        _character.m_velocity += _character.m_gravity;
    }

    public override void update(Character _character)
    {
        if (_character.m_controller.isGrounded && _character.m_direction.magnitude > 0)
        {
            _character.m_animator.speed = 1f;
            _character.m_animator.SetBool("OnGround", true);
            _character.m_currentMovementState = _character.m_statePool[(int)EStates.StandingState];
            _character.m_currentMovementState.enter(_character);
        }

        base.update(_character);
    }
}
