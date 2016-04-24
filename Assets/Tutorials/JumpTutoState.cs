using UnityEngine;
using System.Collections;

public class JumpTutoState : TutoState
{

    UnityEngine.UI.Text m_text;
    Character player;

    float m_time = 0;
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
        Debug.Log("Enter JumpTutoState");
        m_ETutoState = ETutoStates.JumpState;
        m_text.text = "Sometimes it can be useful to be on high ground to get a strategic advantage.";

        GameObject[] goList = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in goList)
        {
            player = go.GetComponent<Character>();
            if (player && player.hasAuthority)
                break;
        }

        m_pakkuAnimator.SetBool("Jump", true);

        base.enter();
    }

    public override void update()
    {
        m_time += Time.deltaTime;

        if (m_part1 && m_time >= m_durationPart1)
        {
            m_part1 = false;
            m_text.text = "To get on high ground try to jump by pressing [Space].";
            m_time = 0;
        }
        else if (m_time >= m_durationPart2 && player.m_currentMovementState
                && player.m_currentMovementState.m_EState == EStates.JumpingState)
        {
            exit();
        }

        base.update();
    }

    public override void exit()
    {
        m_tutoInfo.m_currentState = m_tutoInfo.m_statePool[(int)ETutoStates.DodgeState];
        m_tutoInfo.m_currentState.enter();

        base.exit();
    }
}
