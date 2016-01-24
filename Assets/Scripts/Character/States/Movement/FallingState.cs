using UnityEngine;
using System.Collections;

public class FallingState : CharacterState
{

    public override void enter(Character _character)
    {
        Debug.Log("Enter FallingState");
        m_EState = EStates.FallingState;

        //FALL

        base.enter(_character);
    }

    public override void handleMovement(Character _character, EMovement _movement)
    {
        switch (_movement)
        {
            case EMovement.Stabilize:
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.JumpDescendingState];
                _character.m_currentMovementState.enter(_character);
                break;
        }
    }

    public override void fixedUpdate(Character _character)
    {
        //COMPUTE PHYSIC TO FALL
        _character.m_velocity += _character.m_gravity;
    }

    public override void update(Character _character)
    {
        //if (GROUND REACHED)
        {
            //APPLY DAMAGE
        }

        base.update(_character);
    }
}
