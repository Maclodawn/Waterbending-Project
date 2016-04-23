using UnityEngine;
using System.Collections;

public class DyingState : CharacterState
{ 
    public override void enter()
    {
        Debug.Log("Enter DyingState");
        m_EState = EStates.DyingState;
        print("SET DEAD");
        m_character.m_animator.SetTrigger("Dead");

        base.enter();
    }

    public override void handleMovement(EMovement _movement)
    {
        switch (_movement)
        {
            case EMovement.Revive:
                m_character.m_currentMovementState.exit();
                m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.StandingState];
                m_character.m_currentMovementState.enter();
                break;
        }
    }

    public override void fixedUpdate()
    {

    }

    public override void update()
    {
        base.update();
    }

    public override void exit()
    {
        m_character.m_animator.SetTrigger("Revive");

        base.exit();
    }
}
