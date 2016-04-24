using UnityEngine;
using System.Collections;

public class ActionTutoState : TutoState
{

    UnityEngine.UI.Text m_text;

    float m_time = 0;
    public float m_duration = 2;

    protected override void Start()
    {
        m_text = GetComponentInChildren<UnityEngine.UI.Text>();

        base.Start();
    }

    public override void enter()
    {
        Debug.Log("Enter ActionTutoState");
        m_ETutoState = ETutoStates.ActionState;
        m_text.text = "Now you're ready to do learn some basic water bending moves.";

        m_pakkuAnimator.SetBool("Action", true);

        base.enter();
    }

    public override void update()
    {
        m_time += Time.deltaTime;
        if (m_time >= m_duration)
            exit();

        base.update();
    }

    public override void exit()
    {
        m_tutoInfo.m_currentState = m_tutoInfo.m_statePool[(int)ETutoStates.PullState];
        m_tutoInfo.m_currentState.enter();

        base.exit();
    }
}
