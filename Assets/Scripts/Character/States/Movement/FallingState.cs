using UnityEngine;
using System.Collections;

public class FallingState : CharacterState
{

    public override void enter()
    {
        Debug.Log("Enter FallingState");
        m_EState = EStates.FallingState;
        m_character.m_animator.SetBool("Fall", true);
        m_character.m_controller.height = 0;

        base.enter();
    }

    public override void handleMovement(EMovement _movement)
    {
        switch (_movement)
        {
            case EMovement.Stabilize:
                m_character.m_currentMovementState.exit();
                m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.JumpDescendingState];
                m_character.m_currentMovementState.enter();
                break;
        }
    }

    public override void fixedUpdate()
    {
        m_character.m_velocity += m_character.m_gravity;
    }

    public override void update()
    {
        if (Physics.Raycast(transform.position + Vector3.up, -Vector3.up, 1.0f))
        {
            m_character.m_currentMovementState.exit();
            m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.FallenState];
            m_character.m_currentMovementState.enter();
        }

        base.update();
    }

    public override void exit()
    {
        m_character.m_animator.SetBool("Fall", false);
        m_character.m_controller.height = 1.8f;

        base.exit();
    }
}
