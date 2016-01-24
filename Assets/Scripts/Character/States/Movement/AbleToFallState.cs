using UnityEngine;
using System.Collections;

public class AbleToFallState : CharacterState
{

    public void fall(Character _character)
    {
        _character.m_currentActionState = null;
        _character.m_currentMovementState = _character.m_statePool[(int)EStates.FallingState];
        _character.m_currentMovementState.enter(_character);
    }
}
