using UnityEngine;
using System.Collections;

public class AbleToFallState : CharacterState
{

    public void fall()
    {
        if (m_character.m_currentActionState)
        {
            m_character.m_currentActionState.exit();
        }
        m_character.m_currentMovementState.exit();
        m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.FallingState];
        m_character.m_currentMovementState.enter();
    }
}
