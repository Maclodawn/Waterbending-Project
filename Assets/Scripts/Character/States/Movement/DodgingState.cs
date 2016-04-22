using UnityEngine;
using System.Collections;

public class DodgingState : AbleToFallState
{

    public float m_dodgeRollSpeed = 3;
    public float m_dodgeGetUpSpeed = 1;
    public float m_radius = 1f;
    public float m_height = 1f;

    public override void enter()
    {
        Debug.Log("Enter DodgingState");
        m_character.m_currentMoveSpeed = m_dodgeRollSpeed;
        m_character.m_animator.SetBool("Dodge", true);
        setOrientation();

        base.enter();
    }

    public override void fixedUpdate()
    {
        m_character.m_velocity = Vector3.zero;

        if (m_character.m_animator.GetCurrentAnimatorStateInfo(0).IsName("DodgeRoll"))
        {
            m_character.m_currentMoveSpeed = m_dodgeRollSpeed;
            m_character.m_controller.height = m_height;
        }
        else if (m_character.m_animator.GetCurrentAnimatorStateInfo(0).IsName("DodgeGetUp"))
        {
            m_character.m_currentMoveSpeed = m_dodgeGetUpSpeed;
            m_character.m_controller.height = m_character.m_heightController;
        }

        m_character.m_velocity.z = m_character.m_currentMoveSpeed;
        m_character.m_velocity += m_character.m_gravity;

        base.fixedUpdate();
    }

    public override void update()
    {
        if (m_character.m_animator.GetCurrentAnimatorStateInfo(0).IsName("TempDodge"))
        {
            m_character.m_currentMovementState.exit();
            m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.StandingState];
             m_character.m_currentMovementState.enter();
        }

        base.update();
    }

    public override void exit()
    {
        m_character.m_animator.SetBool("Dodge", false);

        m_character.m_controller.radius = m_character.m_radiusController;
        m_character.m_controller.height = m_character.m_heightController;

        base.exit();
    }

    private void setOrientation()
    {
        ComputeActionsFromInput player = (ComputeActionsFromInput)m_character;

        Vector2 currentForward = new Vector2(transform.forward.x, transform.forward.z);
        float currentAngle = MathHelper.angle(Vector2.up, currentForward);

        float offsetAngle = MathHelper.angle(Vector2.up, m_character.m_inputDirection);

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
