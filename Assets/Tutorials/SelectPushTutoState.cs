using UnityEngine;
using System.Collections;

public class SelectPushTutoState : TutoState
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
        Debug.Log("Enter SelectPushTutoState");
        m_ETutoState = ETutoStates.SelectPushState;
        m_text.text = "Release [Left Click] to throw it.";

        GameObject[] goList = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in goList)
        {
            player = go.GetComponent<Character>();
            if (player && player.hasAuthority)
                break;
        }

        m_pakkuAnimator.SetBool("SelectPush", true);

        base.enter();
    }

    public override void update()
    {
        m_time += Time.deltaTime;
        if (m_time >= m_duration && player.m_currentActionState && player.m_currentActionState.m_EState == EStates.PushingWaterState)
        {
            exit();
        }

        base.update();
    }

    public override void exit()
    {
        m_tutoInfo.m_currentState = m_tutoInfo.m_statePool[(int)ETutoStates.GuardState];
        m_tutoInfo.m_currentState.enter();

        base.exit();
    }
}
