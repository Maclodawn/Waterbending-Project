using UnityEngine;
using System.Collections;

public class StandingState : AbleToJumpState
{

    public override void enter()
    {
        Debug.Log("Enter StandingState");
        m_EState = EStates.StandingState;

        if (m_gettingUp)
        {
            m_character.m_animator.SetBool("GetUp", true);
            m_character.m_controller.center = Vector3.up * 1.6f;
        }

        m_character.m_animator.SetBool("None", true);

        base.enter();
    }

    public override void handleMovement(EMovement _movement)
    {
        switch (_movement)
        {
            case EMovement.Run:
                // We do not want to be able to change the movement to Run if TurningWaterAroundState action is not over
                if (m_character.m_currentActionState is TurningWaterAroundState)
                {
                    return;
                }
                m_character.m_currentMovementState.exit();
                m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.RunningState];
                m_character.m_currentMovementState.enter();
                break;
            case EMovement.Sprint:
                // We do not want to be able to change the movement to Sprint if any action is not over
                if (m_character.m_currentActionState != null)
                {
                    return;
                }

                m_character.m_currentMovementState.exit();
                m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.SprintingState];
                m_character.m_currentMovementState.enter();
                break;
        }
        base.handleMovement(_movement);
    }

    public override void fixedUpdate()
    {
        initFixedUpdate();

        base.fixedUpdate();
    }

    public override void update()
    {
        if (m_gettingUp && m_character.m_animator.GetCurrentAnimatorStateInfo(0).IsName("GetUp"))
        {
            m_gettingUp = false;
            m_character.m_animator.SetBool("GetUp", false);
            m_character.m_controller.center = Vector3.up * 0.9f;
        }

        base.update();
    }

    public override void exit()
    {
        m_character.m_animator.SetBool("None", false);

        base.exit();
    }
}
