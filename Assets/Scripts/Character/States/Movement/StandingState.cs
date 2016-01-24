﻿using UnityEngine;
using System.Collections;

public class StandingState : AbleToJumpState
{

    public override void enter(Character _character)
    {
        Debug.Log("Enter StandingState");
        m_EState = EStates.StandingState;

        //STAND
        _character.m_animator.SetBool("Idle", true);

        base.enter(_character);
    }

    public override void handleMovement(Character _character, EMovement _movement)
    {
        switch (_movement)
        {
            case EMovement.Run:
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.RunningState];
                _character.m_currentMovementState.enter(_character);
                break;
            case EMovement.Sprint:
                // We do not want to be able to change the movement to Sprint if any action is not over
                if (_character.m_currentActionState != null)
                {
                    return;
                }

                _character.m_currentMovementState = _character.m_statePool[(int)EStates.SprintingState];
                _character.m_currentMovementState.enter(_character);
                break;
        }
        base.handleMovement(_character, _movement);
    }

    public override void update(Character _character)
    {
        initUpdate(_character);

        _character.m_animator.SetFloat("Forward", _character.m_localDirection.z, 0.1f, Time.deltaTime);
        _character.m_animator.SetFloat("Turn", Mathf.Atan2(_character.m_localDirection.x,
                                                           _character.m_localDirection.z), 0.1f, Time.deltaTime);

        base.update(_character);
    }
}
