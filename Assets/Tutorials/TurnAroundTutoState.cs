using UnityEngine;
using System.Collections;

public class TurnAroundTutoState : TutoState
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
        Debug.Log("Enter TurnAroundTutoState");
        m_ETutoState = ETutoStates.GreetingsState;
        m_text.text = "Now try to turn the water around by holding [Right Click] pressed and then holding ..FIXME";

        m_pakkuAnimator.SetBool("TurnAround", true);

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
        m_tutoInfo.m_currentState = m_tutoInfo.m_statePool[(int)ETutoStates.TurnAroundState];
        m_tutoInfo.m_currentState.enter();

        base.exit();
    }
}
