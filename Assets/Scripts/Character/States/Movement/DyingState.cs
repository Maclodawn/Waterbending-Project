using UnityEngine;
using System.Collections;

public class DyingState : CharacterState
{
    public override void enter(Character _character)
    {
        Debug.Log("Enter DyingState");
        m_EState = EStates.DyingState;
        _character.m_animator.SetTrigger("Dead");

        Manager.getInstance().ShowDeathUI(_character.GetComponent<ComputeActionsFromInput>(), true);
        Manager.getInstance().ShowCursor();

        base.enter(_character);
    }

    public override void handleMovement(Character _character, EMovement _movement)
    {
        switch (_movement)
        {
            case EMovement.Revive:
                _character.m_currentMovementState.exit(_character);
                _character.m_currentMovementState = _character.m_statePool[(int)EStates.StandingState];
                _character.m_currentMovementState.enter(_character);
                break;
        }
    }

    public override void fixedUpdate(Character _character)
    {

    }

    public override void update(Character _character)
    {
        base.update(_character);
    }

    public override void exit(Character _character)
    {
        _character.m_animator.SetTrigger("Revive");

        _character.GetComponent<ComputeActionsFromInput>().Revive();

        GameObject[] spawns = GameObject.FindGameObjectsWithTag("Respawn");
        int randId = (int)Random.Range(0, spawns.Length);
        _character.transform.position = spawns[randId].transform.position;

        Manager.getInstance().ShowDeathUI(_character.GetComponent<ComputeActionsFromInput>(), false);
        Manager.getInstance().HideCursor();
        base.exit(_character);
    }
}
