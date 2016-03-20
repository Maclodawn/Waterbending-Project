using UnityEngine;
using System.Collections;

public class ByeTutoState : TutoState
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
        Debug.Log("Enter ByeState");
        m_ETutoState = ETutoStates.ByeState;
        m_text.text = "Not bad, not bad! Keep practicing and maybe you'll get it by the time you're my age.";

        m_pakkuAnimator.SetBool("Bye", true);

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
        m_tutoInfo.m_currentState = m_tutoInfo.m_statePool[(int)ETutoStates.PakkuDisappearState];
        m_tutoInfo.m_currentState.enter();

        base.exit();
    }
}
