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

        _character.m_velocity.y = m_jumpSpeed;
        _character.m_animator.Play("Jump");
        _character.m_animator.CrossFade("Grounded", 1f);

        _character.m_animator.SetFloat("Jump", _character.m_velocity.y);

        float runCycle = Mathf.Repeat(_character.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.2f, 1);

        if (runCycle < 0.5f)
        {
            _character.m_animator.SetFloat("JumpLeg", _character.m_localDirection.z);
        }
        else
        {
            _character.m_animator.SetFloat("JumpLeg", -_character.m_localDirection.z);
        }

        base.enter(_character);
    }

    public override void fixedUpdate(Character _character)
    {
        //COMPUTE PHYSIC TO JUMP-ASCEND
    }

    public override void update(Character _character)
    {
        _character.m_animator.SetFloat("Jump", _character.m_controller.velocity.y);
        if (_character.m_controller.velocity.y <= 0)
        {
            _character.m_currentMovementState = _character.m_statePool[(int)EStates.JumpDescendingState];
            _character.m_currentMovementState.enter(_character);
        }

        _character.m_velocity += _character.m_gravity;

        base.update(_character);
    }
}
