using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TurningWaterAroundState : AbleToFallState
{
    public float m_radiusToTurnAround = 1;
    public Vector3 m_offsetToFling;

    [Client]
    public override void enter(Character _character)
    {
        Debug.Log("Enter TurningWaterAroundState");
        m_EState = EStates.TurningWaterAroundState;

        _character.m_waterGroup.m_target.transform.position = _character.transform.position + _character.m_controller.center;
        CmdEnter(_character.GetComponent<NetworkIdentity>(), _character.m_waterGroup.m_target.transform.position, m_radiusToTurnAround);
        
        base.enter(_character);
    }

    [Command]
    void CmdEnter(NetworkIdentity _characterIdentity, Vector3 _targetPos, float _radiusToTurnAround)
    {
        Character character = _characterIdentity.GetComponent<Character>();
        character.m_waterGroup.m_target.transform.position = _targetPos;
        character.m_waterGroup.stopAndTurnAround(_radiusToTurnAround);
    }

    [Client]
    public override void handleAction(Character _character, EAction _action)
    {
        switch(_action)
        {
            case EAction.PushWater:
                _character.m_currentActionState = _character.m_statePool[(int)EStates.PushingWaterState];

                PushingWaterState pushingWaterState = (_character.m_currentActionState as PushingWaterState);
                pushingWaterState.init(m_offsetToFling, 0, true);

                _character.m_currentActionState.enter(_character);
                break;
        }

        base.handleAction(_character, _action);
    }

    public override void exit(Character _character)
    {
        _character.m_currentActionState = null;

        base.exit(_character);
    }
}
