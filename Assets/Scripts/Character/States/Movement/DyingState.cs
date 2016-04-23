using UnityEngine;
using System.Collections;

public class DyingState : CharacterState
{
    public override void enter(Character _character)
    {
        Debug.Log("Enter DyingState");
        m_EState = EStates.DyingState;
        print("SET DEAD");
        _character.m_animator.SetTrigger("Dead");

        base.enter(_character);
    }

    public override void handleMovement(Character _character, EMovement _movement)
    {
        switch (_movement)
        {
            case EMovement.Revive:
                _character.m_currentMovementState.exit(_character);
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.StandingState];
                _character.m_currentMovementState.enter(_character);
                break;
        }
    }

    public override void fixedUpdate(Character _character)
    {

    }

    public override void update(Character _character)
    {
        base.update(_character);
    }

    public override void exit(Character _character)
    {
        _character.m_animator.SetTrigger("Revive");

        base.exit(_character);
    }
}
