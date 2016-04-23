using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GuardingState : AbleToFallState
{

    GameObject m_waterDeflectGuard;

    public override void enter()
    {
        Debug.Log("Enter GuardingState");

        // ANIMATION

        CmdEnter();
        
        base.enter();
    }

    [Command]
    void CmdEnter()
    {
        if (!m_waterDeflectGuard)
        {
            m_waterDeflectGuard = Instantiate(Manager.getInstance().m_waterDeflectGuardPrefab);
            m_waterDeflectGuard.transform.position = transform.position + m_character.m_controller.center;
        }
        else
        {
            m_waterDeflectGuard.SetActive(true);
            m_waterDeflectGuard.transform.position = transform.position + m_character.m_controller.center;
        }
    }

    public override void handleAction(EAction _action)
    {
        switch(_action)
        {
            case EAction.ReleaseGuard:
                exit();
                break;
        }

        base.handleAction(_action);
    }

    public override void update()
    {
        CmdUpdate();

        base.update();
    }

    [Command]
    void CmdUpdate()
    {
        m_waterDeflectGuard.transform.position = transform.position + m_character.m_controller.center;
    }

    public override void exit()
    {
        print("Release Guard");
        CmdExit();
        m_character.m_currentActionState = null;
        
        base.exit();
    }

    [Command]
    void CmdExit()
    {
        m_waterDeflectGuard.SetActive(false);
    }
}
