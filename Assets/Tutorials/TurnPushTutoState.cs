using UnityEngine;
using System.Collections;

public class TurnPushTutoState : TutoState
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
        Debug.Log("Enter TurnPushTutoState");
        m_ETutoState = ETutoStates.TurnPushState;
        m_text.text = "You can now throw the water wherever you aim by releasing [Right Click] and [Left Click].";

        GameObject[] goList = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in goList)
        {
            player = go.GetComponent<Character>();
            if (player && player.hasAuthority)
                break;
        }

        m_pakkuAnimator.SetBool("TurnPush", true);

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
        m_tutoInfo.m_currentState = m_tutoInfo.m_statePool[(int)ETutoStates.SelectState];
        m_tutoInfo.m_currentState.enter();

        base.exit();
    }
}
