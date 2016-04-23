using UnityEngine;
using System.Collections;

public class SprintingState : RunSprintingState
{
    [SerializeField]
    private float m_sprintSpeed = 14.0f;

    public override void enter()
    {
        Debug.Log("Enter SprintingState");
        m_EState = EStates.SprintingState;
        m_character.m_animator.SetBool("Sprint", true);

        base.enter();
    }

    public override void handleAction(EAction _action)
    {
        switch(_action)
        {
            case EAction.SelectWaterToPush:
            case EAction.PullWater:
                m_character.m_currentMovementState.exit();
                m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.RunningState];
                m_character.m_currentMovementState.enter();
                break;
        }

        base.handleAction(_action);
    }

    public override void handleMovement(EMovement _movement)
    {
        switch(_movement)
        {
            case EMovement.Run:
                m_character.m_currentMovementState.exit();
                m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.RunningState];
                m_character.m_currentMovementState.enter();
                break;
        }
        base.handleMovement(_movement);
    }

    public override void fixedUpdate()
    {
        initFixedUpdate();

        m_character.m_currentMoveSpeed = m_sprintSpeed;

        base.fixedUpdate();
    }

    public override void exit()
    {
        m_character.m_animator.SetBool("Sprint", false);

        base.exit();
    }
}
