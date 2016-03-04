using UnityEngine;
using System.Collections;

public class SprintingState : RunSprintingState
{
    [SerializeField]
    private float m_sprintSpeed = 14.0f;

    public override void enter(Character _character)
    {
        Debug.Log("Enter SprintingState");
        m_EState = EStates.SprintingState;
        _character.m_animator.SetBool("Sprint", true);

        base.enter(_character);
    }

    public override void handleAction(Character _character, EAction _action)
    {
        switch(_action)
        {
            case EAction.SelectWaterToPush:
            case EAction.PullWater:
                _character.m_currentMovementState.exit(_character);
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.RunningState];
                _character.m_currentMovementState.enter(_character);
                break;
        }

        base.handleAction(_character, _action);
    }

    public override void handleMovement(Character _character, EMovement _movement)
    {
        switch(_movement)
        {
            case EMovement.Run:
                _character.m_currentMovementState.exit(_character);
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.RunningState];
                _character.m_currentMovementState.enter(_character);
                break;
        }
        base.handleMovement(_character, _movement);
    }

    public override void fixedUpdate(Character _character)
    {
        initFixedUpdate(_character);

        _character.m_currentMoveSpeed = m_sprintSpeed;

        base.fixedUpdate(_character);
    }

    public override void exit(Character _character)
    {
        _character.m_animator.SetBool("Sprint", false);

        base.exit(_character);
    }
}
