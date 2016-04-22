using UnityEngine;
using System.Collections;

public class FallenState : CharacterState
{

    public override void enter()
    {
        Debug.Log("Enter FallenState");
        m_EState = EStates.FallenState;
        m_character.m_animator.SetBool("Fallen", true);
        m_character.m_controller.height = 0;
        m_character.m_controller.center = Vector3.up * 1.6f;

        base.enter();
    }

    public override void handleMovement(EMovement _movement)
    {
        switch (_movement)
        {
            case EMovement.Stabilize:
                m_character.m_currentMovementState.exit();
                m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.StandingState];
                ((StandingState)m_character.m_currentMovementState).m_gettingUp = true;
                m_character.m_currentMovementState.enter();
                break;
        }
    }

    public override void exit()
    {
        m_character.m_animator.SetBool("Fallen", false);
        m_character.m_controller.height = 1.8f;
        m_character.m_controller.center = Vector3.up * 0.9f;

        base.exit();
    }
}
