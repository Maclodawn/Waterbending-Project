using UnityEngine;
using System.Collections;

public class DodgingState : AbleToFallState
{

    public override void enter(Character _character)
    {
        Debug.Log("Enter DodgingState");

        base.enter(_character);
    }

    public override void fixedUpdate(Character _character)
    {
        //FIXME Compute Physic

        base.fixedUpdate(_character);
    }

    public override void update(Character _character)
    {
//         if (Animation is Over)
//         {
//             _character.m_currentMovementState.exit(_character);
//             _character.m_currentMovementState = _character.m_statePool[(int)EStates.StandingState];
//             _character.m_currentMovementState.enter(_character);
//         }

        base.update(_character);
    }
}
