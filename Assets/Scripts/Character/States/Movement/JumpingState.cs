using UnityEngine;
using System.Collections;

public class JumpingState : CharacterState
{
    [SerializeField]
    private float m_jumpSpeed = 7.0f;

    public override void enter()
    {
        Debug.Log("Enter JumpingState");
        m_EState = EStates.JumpingState;
        m_character.m_animator.SetBool("Jump", true);
        m_character.m_controller.height = 1.3f;

        m_character.m_velocity.y = m_jumpSpeed;

        base.enter();
    }

    public override void update()
    {
        if (m_character.m_controller.velocity.y < 0)
        {
            m_character.m_currentMovementState.exit();
            m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.JumpDescendingState];
            m_character.m_currentMovementState.enter();
        }

        m_character.m_velocity += m_character.m_gravity;

        base.update();
    }

    public override void exit()
    {
        m_character.m_controller.height = 1.8f;

        base.exit();
    }
}
