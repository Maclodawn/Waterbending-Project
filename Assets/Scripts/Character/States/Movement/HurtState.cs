using UnityEngine;
using System.Collections;

public class HurtState : CharacterState
{
    public float duration;
    private float time;

    public override void enter()
    {
        Debug.Log("Enter HurtState");
        m_EState = EStates.HurtState;
        m_character.m_animator.SetBool("Hurt", true);
        time = duration;
        m_character.m_velocity = Vector3.zero;

        base.enter();
    }

    public override void handleMovement(EMovement _movement)
    {

    }

    public override void fixedUpdate()
    {

    }

    public override void update()
    {
        time -= Time.deltaTime;
        if(time <= 0)
        {
            m_character.m_currentMovementState.exit();
            m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.StandingState];
            m_character.m_currentMovementState.enter();
        }
        base.update();
    }

    public override void exit()
    {
        m_character.m_animator.SetBool("Hurt", false);

        base.exit();
    }
}
