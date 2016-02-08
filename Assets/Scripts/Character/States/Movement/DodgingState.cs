using UnityEngine;
using System.Collections;

public class DodgingState : AbleToFallState
{

    public float m_dodgeRollSpeed = 3;
    public float m_dodgeGetUpSpeed = 1;
    public float m_radius = 1f;
    public float m_height = 1f;

    public override void enter(Character _character)
    {
        Debug.Log("Enter DodgingState");
        _character.m_currentMoveSpeed = m_dodgeRollSpeed;
        _character.m_animator.SetBool("Dodge", true);
        //Debug.Break();
        setOrientation(_character);

        base.enter(_character);
    }

    public override void fixedUpdate(Character _character)
    {
        _character.m_velocity = Vector3.zero;

        if (_character.m_animator.GetCurrentAnimatorStateInfo(0).IsName("DodgeRoll"))
        {
            _character.m_currentMoveSpeed = m_dodgeRollSpeed;
            _character.m_controller.height = m_height;
        }
        else if (_character.m_animator.GetCurrentAnimatorStateInfo(0).IsName("DodgeGetUp"))
        {
            _character.m_currentMoveSpeed = m_dodgeGetUpSpeed;
            _character.m_controller.height = _character.m_heightController;
        }

        _character.m_velocity.z = _character.m_currentMoveSpeed;
        _character.m_velocity += _character.m_gravity;

        base.fixedUpdate(_character);
    }

    public override void update(Character _character)
    {
        if (_character.m_animator.GetCurrentAnimatorStateInfo(0).IsName("TempDodge"))
        {
            _character.m_currentMovementState.exit(_character);
            _character.m_currentMovementState = _character.m_statePool[(int)EStates.StandingState];
             _character.m_currentMovementState.enter(_character);
        }

        base.update(_character);
    }

    public override void exit(Character _character)
    {
        _character.m_animator.SetBool("Dodge", false);

        _character.m_controller.radius = _character.m_radiusController;
        _character.m_controller.height = _character.m_heightController;

        base.exit(_character);
    }

    private void setOrientation(Character _character)
    {
        ComputeActionsFromInput player = (ComputeActionsFromInput)_character;

        Vector2 currentForward = new Vector2(transform.forward.x, transform.forward.z);
        float currentAngle = MathHelper.angle(Vector2.up, currentForward);

        float offsetAngle = MathHelper.angle(Vector2.up, _character.m_inputDirection);

        float angle;
        if (player == null)
        {
            angle = offsetAngle - currentAngle;
        }
        else
        {
            Vector2 cameraDirection = new Vector2(player.m_cameraTransform.forward.x, player.m_cameraTransform.forward.z);
            float cameraAngle = MathHelper.angle(Vector2.up, cameraDirection);

            angle = cameraAngle + offsetAngle - currentAngle;
        }

        angle = Mathf.LerpAngle(0, angle, 0.3f);
        transform.Rotate(Vector3.up, angle);
    }
}
