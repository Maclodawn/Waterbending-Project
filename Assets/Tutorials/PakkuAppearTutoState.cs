using UnityEngine;
using System.Collections;

public class PakkuAppearTutoState : TutoState
{

    UnityEngine.UI.RawImage m_bubble;

    protected override void Start()
    {
        m_bubble = GetComponentInChildren<UnityEngine.UI.RawImage>();

        base.Start();
    }

    public override void enter()
    {
        Debug.Log("Enter PakkuAppearState");
        m_ETutoState = ETutoStates.PakkuAppearState;

        m_pakkuAnimator.SetBool("Pop", true);

        base.enter();
    }

    public override void update()
    {
        if (m_bubble.enabled)
        {
            exit();
        }

        base.update();
    }

    public override void exit()
    {
        m_tutoInfo.m_currentState = m_tutoInfo.m_statePool[(int)ETutoStates.GreetingsState];
        m_tutoInfo.m_currentState.enter();

        base.exit();
    }
}
