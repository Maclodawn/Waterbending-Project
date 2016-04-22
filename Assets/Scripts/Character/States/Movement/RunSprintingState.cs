using UnityEngine;
using System.Collections;

public class RunSprintingState : AbleToJumpState
{

    public override void handleAction(EAction _action)
    {
        switch (_action)
        {
            case EAction.TurnWaterAround:
                m_character.m_currentMovementState.exit();
                m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.StandingState];
                m_character.m_currentMovementState.enter();
                break;
        }

        base.handleAction(_action);
    }

    public override void handleMovement(EMovement _movement)
    {
        switch (_movement)
        {
            case EMovement.None:
                m_character.m_currentMovementState.exit();
                m_character.m_currentMovementState = m_character.m_statePool[(int)EStates.StandingState];
                m_character.m_currentMovementState.enter();
                break;
        }
        base.handleMovement(_movement);
    }

    public override void fixedUpdate()
    {
        m_character.m_velocity.z = m_character.m_currentMoveSpeed;

        setOrientation();

        base.fixedUpdate();
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
