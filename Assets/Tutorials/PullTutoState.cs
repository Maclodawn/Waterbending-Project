using UnityEngine;
using System.Collections;

public class PullTutoState : TutoState
{

    UnityEngine.UI.Text m_text;
    Character player;

    float m_time = 0;
    public float m_duration = 2;

    protected override void Start()
    {
        m_text = GetComponentInChildren<UnityEngine.UI.Text>();

        base.Start();
    }

    public override void enter()
    {
        Debug.Log("Enter PullState");
        m_ETutoState = ETutoStates.PullState;
        m_text.text = "Hold [Right Click] to pull some water.";

        GameObject[] goList = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in goList)
        {
            player = go.GetComponent<Character>();
            if (player && player.hasAuthority)
                break;
        }

        m_pakkuAnimator.SetBool("Pull", true);

        base.enter();
    }

    public override void update()
    {
        m_time += Time.deltaTime;
        if (m_time >= m_duration && player.m_currentActionState && player.m_currentActionState.m_EState == EStates.PullingWaterState)
       {
           exit();
       }

        base.update();
    }

    public override void exit()
    {
        m_tutoInfo.m_currentState = m_tutoInfo.m_statePool[(int)ETutoStates.TurnAroundState];
        m_tutoInfo.m_currentState.enter();

        base.exit();
    }
}
