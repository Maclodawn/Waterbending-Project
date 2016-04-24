using UnityEngine;
using System.Collections;

public class CounterTutoState : TutoState
{

    UnityEngine.UI.Text m_text;
    Character player;

    float m_time = 0;
    public float m_duration = 2;
    [System.NonSerialized]
    public bool m_countered = false;

    protected override void Start()
    {
        m_text = GetComponentInChildren<UnityEngine.UI.Text>();

        base.Start();
    }

    public override void enter()
    {
        Debug.Log("Enter CounterTutoState");
        m_ETutoState = ETutoStates.CounterState;
        m_text.text = "If you're good enough to feel the flow maybe you'll be able to redirect "
                    + "the water to your opponent by pressing [F] at the right time!";

        m_pakkuAnimator.SetBool("Counter", true);

        base.enter();
    }

    public override void update()
    {
        m_time += Time.deltaTime;
        if (m_time >= m_duration && m_countered)
        {
            exit();
        }

        base.update();
    }

    public override void exit()
    {
        m_tutoInfo.m_currentState = m_tutoInfo.m_statePool[(int)ETutoStates.ByeState];
        m_tutoInfo.m_currentState.enter();

        base.exit();
    }
}
