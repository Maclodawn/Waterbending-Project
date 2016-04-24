using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TurningWaterAroundState : AbleToFallState
{
    public float m_radiusToTurnAround = 1;
    public Vector3 m_offsetToFling;
    private bool m_countering = false;

    [Client]
    public void initCounter()
    {
        m_countering = true;
    }

    [Client]
    public override void enter()
    {
        Debug.Log("Enter TurningWaterAroundState");
        m_EState = EStates.TurningWaterAroundState;

        if (m_character.m_waterGroup == null)
            exit();
        m_character.m_waterGroup.m_target.transform.position = m_character.transform.position;
        Character targetCharacter = m_character.m_waterGroup.m_target.GetComponent<Character>();
        if (!targetCharacter)
            m_character.m_waterGroup.m_target.transform.position += m_character.m_controller.center;
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
        if (_action == EAction.PushWater || m_countering)
        {
            m_character.m_currentActionState = m_character.m_statePool[(int)EStates.PushingWaterState];

            PushingWaterState pushingWaterState = (m_character.m_currentActionState as PushingWaterState);
            pushingWaterState.init(m_offsetToFling, 0, true);

            m_character.m_currentActionState.enter();
        }

        base.handleAction(_action);
    }

    public override void exit()
    {
        m_countering = false;
        m_character.m_currentActionState = null;
        m_character.m_waterGroup.releaseControl();

        base.exit();
    }
}
