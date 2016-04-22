using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TurningWaterAroundState : AbleToFallState
{
    public float m_radiusToTurnAround = 1;
    public Vector3 m_offsetToFling;

    [Client]
    public override void enter()
    {
        Debug.Log("Enter TurningWaterAroundState");
        m_EState = EStates.TurningWaterAroundState;

        m_character.m_waterGroup.m_target.transform.position = m_character.transform.position + m_character.m_controller.center;
        CmdEnter(m_character.m_waterGroup.m_target.transform.position, m_radiusToTurnAround);
        
        base.enter();
    }

    [Command]
    void CmdEnter(Vector3 _targetPos, float _radiusToTurnAround)
    {
        m_character.m_waterGroup.m_target.transform.position = _targetPos;
        m_character.m_waterGroup.stopAndTurnAround(_radiusToTurnAround);
    }

    [Client]
    public override void handleAction(EAction _action)
    {
        switch(_action)
        {
            case EAction.PushWater:
                m_character.m_currentActionState = m_character.m_statePool[(int)EStates.PushingWaterState];

                PushingWaterState pushingWaterState = (m_character.m_currentActionState as PushingWaterState);
                pushingWaterState.init(m_offsetToFling, 0, true);

                m_character.m_currentActionState.enter();
                break;
        }

        base.handleAction(_action);
    }

    public override void exit()
    {
        m_character.m_currentActionState = null;

        base.exit();
    }
}
