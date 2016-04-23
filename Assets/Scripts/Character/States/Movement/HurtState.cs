using UnityEngine;
using System.Collections;

public class HurtState : AbleToFallState
{
    public float duration = 0.5f;
    private float time;

    public override void enter(Character _character)
    {
        m_EState = EStates.HurtState;
        _character.m_animator.SetBool("Hurt", true);
        time = duration;

        base.enter(_character);
    }

    public override void handleMovement(Character _character, EMovement _movement)
    {
        
    }

    public override void fixedUpdate(Character _character)
    {

    }

    public override void update(Character _character)
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            _character.m_currentMovementState.exit(_character);
            _character.m_currentMovementState = _character.m_statePool[(int)EStates.StandingState];
            _character.m_currentMovementState.enter(_character);
        }
        base.update(_character);
    }

    public override void exit(Character _character)
    {
        _character.m_animator.SetBool("Hurt", false);
        base.exit(_character);
    }
}
