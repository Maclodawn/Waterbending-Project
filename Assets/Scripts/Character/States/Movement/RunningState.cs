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
        _character.m_animator.SetBool("Run", true);

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

                _character.m_currentMovementState.exit(_character);
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

        base.update(_character);
    }

    public override void exit(Character _character)
    {
        _character.m_animator.SetBool("Run", false);

        base.exit(_character);
    }
}
