using UnityEngine;
using System.Collections;

public class StandingState : AbleToJumpState
{

    public override void enter(Character _character)
    {
        Debug.Log("Enter StandingState");
        m_EState = EStates.StandingState;

        if (m_gettingUp)
        {
            _character.m_animator.SetBool("GetUp", true);
            _character.m_controller.center = Vector3.up * 1.6f;
        }

        _character.m_animator.SetBool("None", true);

        base.enter(_character);
    }

    public override void handleMovement(Character _character, EMovement _movement)
    {
        switch (_movement)
        {
            case EMovement.Run:
                _character.m_currentMovementState.exit(_character);
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.RunningState];
                _character.m_currentMovementState.enter(_character);
                break;
            case EMovement.Sprint:
                // We do not want to be able to change the movement to Sprint if any action is not over
                if (_character.m_currentActionState != null)
                {
                    return;
                }

                _character.m_currentMovementState.exit(_character);
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.SprintingState];
                _character.m_currentMovementState.enter(_character);
                break;
        }
        base.handleMovement(_character, _movement);
    }

    public override void fixedUpdate(Character _character)
    {
        initFixedUpdate(_character);

        base.fixedUpdate(_character);
    }

    public override void update(Character _character)
    {
        if (m_gettingUp && _character.m_animator.GetCurrentAnimatorStateInfo(0).IsName("GetUp"))
        {
            m_gettingUp = false;
            _character.m_animator.SetBool("GetUp", false);
            _character.m_controller.center = Vector3.up * 0.9f;
        }

        base.update(_character);
    }

    public override void exit(Character _character)
    {
        _character.m_animator.SetBool("None", false);

        base.exit(_character);
    }
}
