using UnityEngine;
using System.Collections;

public class DeviateTutoState : TutoState
{

    UnityEngine.UI.Text m_text;
    Character player;

    float m_time = 0;
    [System.NonSerialized]
    public bool m_deviated = false;

    public float m_durationPart1 = 2;
    public float m_durationPart2 = 2;

    bool m_part1 = true;

    protected override void Start()
    {
        m_text = GetComponentInChildren<UnityEngine.UI.Text>();

        base.Start();
    }

    public override void enter()
    {
        Debug.Log("Enter DeviateTutoState");
        m_ETutoState = ETutoStates.DeviateState;
        m_text.text = "The key to water bending is timings. Just before the water touch you, press [F] to deviate it.";

        m_pakkuAnimator.SetBool("Deviate", true);

        base.enter();
    }

    public override void update()
    {
        m_time += Time.deltaTime;

        if (m_time >= m_durationPart1 && m_part1)
        {
            m_time = 0;
            m_part1 = false;
            m_text.text = "This will prevent you from losing health but will consume a lot of energy.";
        }
        if (m_time >= m_durationPart2 && m_deviated)
        {
            exit();
        }

        base.update();
    }

    public override void exit()
    {
        m_tutoInfo.m_currentState = m_tutoInfo.m_statePool[(int)ETutoStates.CounterState];
        m_tutoInfo.m_currentState.enter();

        base.exit();
    }
}
