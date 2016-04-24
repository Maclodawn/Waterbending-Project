using UnityEngine;
using System.Collections;

public class RunningState : RunSprintingState
{
    [SerializeField]
    private float m_runSpeed = 7.0f;

    public override void enter()
    {
        Debug.Log("Enter RunnningState");
        m_EState = EStates.RunningState;
        m_character.m_animator.SetBool("Run", true);

        base.enter();
    }

    public override void handleMovement(EMovement _movement)
    {
        switch (_movement)
        {
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
            case EMovement.Hurt:
                m_character.m_currentMovementState.exit();
                m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.HurtState];
                m_character.m_currentMovementState.enter();
                break;

        }
        base.handleMovement(_movement);
    }

    public override void fixedUpdate()
    {
        initFixedUpdate();
        m_character.m_currentMoveSpeed = m_runSpeed;

        base.fixedUpdate();
    }

    public override void exit()
    {
        m_character.m_animator.SetBool("Run", false);

        base.exit();
    }
}
