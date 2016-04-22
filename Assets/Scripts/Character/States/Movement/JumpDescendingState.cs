using UnityEngine;
using System.Collections;

public class JumpDescendingState : AbleToFallState
{

    public override void enter()
    {
        Debug.Log("Enter JumpDescendingState");
        m_EState = EStates.JumpDescendingState;
        m_character.m_animator.SetBool("Jump", true);
        m_character.m_controller.height = 1.3f;

        base.enter();
    }

    public override void fixedUpdate()
    {
        m_character.m_velocity += m_character.m_gravity;
    }

    public override void update()
    {
        if (m_character.m_controller.isGrounded && m_character.m_movementDirection.magnitude > 0)
        {
            m_character.m_currentMovementState.exit();
            m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.StandingState];
            m_character.m_currentMovementState.enter();
        }

        base.update();
    }

    public override void exit()
    {
        m_character.m_animator.SetBool("Jump", false);
        m_character.m_controller.height = 1.8f;

        base.exit();
    }
}
