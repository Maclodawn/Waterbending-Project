using UnityEngine;
using System.Collections;

public class RunSprintingState : AbleToJumpState
{

    public override void enter(Character _character)
    {
        ////Debug.Log(">RunSprintingState");

        base.enter(_character);
    }

    public override void handleAction(Character _character, EAction _action)
    {
        switch (_action)
        {
            case EAction.TurnWaterAround:
                _character.m_currentMovementState.exit(_character);
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.StandingState];
                _character.m_currentMovementState.enter(_character);
                break;
        }

        base.handleAction(_character, _action);
    }

    public override void handleMovement(Character _character, EMovement _movement)
    {
        switch (_movement)
        {
            case EMovement.None:
                _character.m_currentMovementState.exit(_character);
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.StandingState];
                _character.m_currentMovementState.enter(_character);
                break;
        }
        base.handleMovement(_character, _movement);
    }

    public override void fixedUpdate(Character _character)
    {
        _character.m_velocity.z = _character.m_currentMoveSpeed;

        setOrientation(_character);

        base.fixedUpdate(_character);
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
