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

        //PLAY SPRINT ANIMATION

        base.enter(_character);
    }

    public override void handleAction(Character _character, EAction _action)
    {
        switch(_action)
        {
            case EAction.SelectWaterToPush:
            case EAction.PullWater:
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
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.RunningState];
                _character.m_currentMovementState.enter(_character);
                break;
        }
        base.handleMovement(_character, _movement);
    }

    public override void fixedUpdate(Character _character)
    {
        //COMPUTE PHYSIC TO SPRINT
        base.fixedUpdate(_character);
    }

    public override void update(Character _character)
    {
        initUpdate(_character);

        _character.m_currentMoveSpeed = m_sprintSpeed;

        _character.m_animator.SetFloat("Forward", _character.m_localDirection.z, 0.1f, Time.deltaTime);
        _character.m_animator.SetFloat("Turn", Mathf.Atan2(_character.m_localDirection.x,
                                                           _character.m_localDirection.z), 0.1f, Time.deltaTime);

        base.update(_character);
    }
}
