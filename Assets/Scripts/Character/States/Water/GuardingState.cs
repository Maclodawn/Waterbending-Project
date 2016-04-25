using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GuardingState : AbleToFallState
{

    //GameObject m_waterDeflectGuard;
    HealthController m_healthController;

    void Start()
    {
        m_healthController = GetComponent<HealthController>();
    }

    public override void enter()
    {
        Debug.Log("Enter GuardingState");

        m_EState = EStates.GuardingState;

        m_healthController.m_guarding = true;
        CmdEnter();

        GetComponent<Animator>().SetBool("Guard", true);

        base.enter();
    }

    [Command]
    void CmdEnter()
    {
        m_healthController.m_guarding = true;
//         if (!m_waterDeflectGuard)
//         {
//             m_waterDeflectGuard = Instantiate(Manager.getInstance().m_waterDeflectGuardPrefab);
//             m_waterDeflectGuard.transform.position = transform.position + m_character.m_controller.center;
//         }
//         else
//         {
//             m_waterDeflectGuard.SetActive(true);
//             m_waterDeflectGuard.transform.position = transform.position + m_character.m_controller.center;
//         }
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

//     public override void update()
//     {
//         CmdUpdate();
// 
//         base.update();
//     }
// 
//     [Command]
//     void CmdUpdate()
//     {
//         m_waterDeflectGuard.transform.position = transform.position + m_character.m_controller.center;
//     }

    public override void exit()
    {
        print("Release Guard");
        m_healthController.m_guarding = false;
        //CmdExit();
        m_character.m_currentActionState = null;
        GetComponent<Animator>().SetBool("Guard", false);

        base.exit();
    }

//     [Command]
//     void CmdExit()
//     {
//         m_waterDeflectGuard.SetActive(false);
//     }
}
