using UnityEngine;
using System.Collections;

public class FallenState : CharacterState
{

    public override void enter(Character _character)
    {
        Debug.Log("Enter FallenState");
        m_EState = EStates.FallenState;
        _character.m_animator.SetBool("Fallen", true);
        _character.m_controller.height = 0;
        _character.m_controller.center = Vector3.up * 1.6f;

        base.enter(_character);
    }

    public override void handleMovement(Character _character, EMovement _movement)
    {
        switch (_movement)
        {
            case EMovement.Stabilize:
                _character.m_currentMovementState.exit(_character);
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.StandingState];
                ((StandingState)_character.m_currentMovementState).m_gettingUp = true;
                _character.m_currentMovementState.enter(_character);
                break;
        }
    }

    public override void exit(Character _character)
    {
        _character.m_animator.SetBool("Fallen", false);
        _character.m_controller.height = 1.8f;
        _character.m_controller.center = Vector3.up * 0.9f;

        base.exit(_character);
    }
}
