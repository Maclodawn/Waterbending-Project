using UnityEngine;
using System.Collections;

public class JumpingState : CharacterState
{
    [SerializeField]
    private float m_jumpSpeed = 7.0f;

    public override void enter(Character _character)
    {
        Debug.Log("Enter JumpingState");
        m_EState = EStates.JumpingState;
        _character.m_animator.SetBool("Jump", true);
        _character.m_controller.height = 1.3f;

        _character.m_velocity.y = m_jumpSpeed;

        base.enter(_character);
    }

    public override void fixedUpdate(Character _character)
    {
        //COMPUTE PHYSIC TO JUMP-ASCEND
    }

    public override void update(Character _character)
    {
        if (_character.m_controller.velocity.y < 0)
        {
            _character.m_currentMovementState.exit(_character);
            _character.m_currentMovementState = _character.m_statePool[(int)EStates.JumpDescendingState];
            _character.m_currentMovementState.enter(_character);
        }

        _character.m_velocity += _character.m_gravity;

        base.update(_character);
    }

    public override void exit(Character _character)
    {
        _character.m_controller.height = 1.8f;

        base.exit(_character);
    }
}
