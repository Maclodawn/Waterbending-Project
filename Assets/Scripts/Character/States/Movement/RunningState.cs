using UnityEngine;
using System.Collections;

public class RunningState : RunSprintingState
{
    [SerializeField]
    private float m_runSpeed = 7.0f;

    public override void enter(Character _character)
    {
        Debug.Log("Enter RunnningState");
        m_EState = EStates.RunningState;

        base.enter(_character);
    }

    public override void handleMovement(Character _character, EMovement _movement)
    {
        switch (_movement)
        {
            case EMovement.Sprint:
                // We do not want to be able to change the movement to Sprint if these actions are not over
                if (_character.m_currentActionState != null
                    && (_character.m_currentActionState.m_EState == EStates.SelectingWaterToPushState
                        || _character.m_currentActionState.m_EState == EStates.PushingWaterState
                        || _character.m_currentActionState.m_EState == EStates.PullingWaterState
                        || _character.m_currentActionState.m_EState == EStates.TurningWaterAroundState))
                {
                    return;
                }
                
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.SprintingState];
                _character.m_currentMovementState.enter(_character);
                break;
        }
        base.handleMovement(_character, _movement);
    }

    public override void fixedUpdate(Character _character)
    {
        //UPDATE PHYSIC
        base.fixedUpdate(_character);
    }

    public override void update(Character _character)
    {
        initUpdate(_character);
        _character.m_currentMoveSpeed = m_runSpeed;

        //RUN
        _character.m_animator.SetFloat("Forward", _character.m_localDirection.z, 0.1f, Time.deltaTime);
        _character.m_animator.SetFloat("Turn", Mathf.Atan2(_character.m_localDirection.x,
                                                           _character.m_localDirection.z), 0.1f, Time.deltaTime);

        base.update(_character);
    }
}
