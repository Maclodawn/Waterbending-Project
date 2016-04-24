using UnityEngine;
using System.Collections;

public class SelectTutoState : TutoState
{

    UnityEngine.UI.Text m_text;
    Character player;

    float m_time = 0;
    public float m_durationPart1 = 2;
    public float m_durationPart2 = 2;
    public float m_durationPart3 = 2;

    bool m_part1 = true;
    bool m_part2 = true;

    protected override void Start()
    {
        m_text = GetComponentInChildren<UnityEngine.UI.Text>();

        base.Start();
    }

    public override void enter()
    {
        Debug.Log("Enter SelectTutoState");
        m_ETutoState = ETutoStates.SelectState;
        m_text.text = "As [Right Click] is for pulling water, [Left Click] is to push it.";

        GameObject[] goList = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in goList)
        {
            player = go.GetComponent<Character>();
            if (player && player.hasAuthority)
                break;
        }

        m_pakkuAnimator.SetBool("Select1", true);

        base.enter();
    }

    public override void update()
    {
        m_time += Time.deltaTime;
        if (m_time >= m_durationPart1 && m_part1)
        {
            m_part1 = false;
            m_text.text = "You can either aim at some water on the ground and then hold [Left Click] to select this reserve so the water will come out of it;";
            m_time = 0;
            m_pakkuAnimator.SetBool("Select2", true);
        }
        else if (m_time >= m_durationPart2 && m_part2)
        {
            m_part2 = false;
            m_text.text = "Or you can just hold [Left Click] and it will select the nearest reserve.";
            m_time = 0;
            m_pakkuAnimator.SetBool("Select3", true);
        }
        else if (m_time >= m_durationPart3 && player.m_currentActionState
                && player.m_currentActionState.m_EState == EStates.SelectingWaterToPushState)
        {
            exit();
        }

        base.update();
    }

    public override void exit()
    {
        m_tutoInfo.m_currentState = m_tutoInfo.m_statePool[(int)ETutoStates.SelectPushState];
        m_tutoInfo.m_currentState.enter();

        base.exit();
    }
}
