using UnityEngine;
using System.Collections;

public class RunningState : RunSprintingState
{
    [SerializeField]
    private float m_runSpeed = 7.0f;

    public override void enter(Character _character)
    {
        Debug.Log("Enter RunnningState");
        m_EState = EStates.RunningState;

        base.enter(_character);
    }

    public override void handleMovement(Character _character, EMovement _movement)
    {
        switch (_movement)
        {
            case EMovement.Sprint:
                // We do not want to be able to change the movement to Sprint if any action is not over
                if (_character.m_currentActionState != null)
                {
                    return;
                }
                
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.SprintingState];
                _character.m_currentMovementState.enter(_character);
                break;
        }
        base.handleMovement(_character, _movement);
    }

    public override void fixedUpdate(Character _character)
    {
        //UPDATE PHYSIC
        base.fixedUpdate(_character);
    }

    public override void update(Character _character)
    {
        initUpdate(_character);
        _character.m_currentMoveSpeed = m_runSpeed;

        //RUN
        float direction = Mathf.Sqrt(_character.m_inputDirection.y * _character.m_inputDirection.y
                                    + _character.m_inputDirection.x * _character.m_inputDirection.x);
        _character.m_animator.SetFloat("Direction", direction);

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

        //         _character.m_animator.SetFloat("Forward", _character.m_localDirection.z, 0.1f, Time.deltaTime);
//         _character.m_animator.SetFloat("Turn", Mathf.Atan2(_character.m_localDirection.x,
//                                                            _character.m_localDirection.z), 0.1f, Time.deltaTime);

        base.update(_character);
    }
}
