using UnityEngine;
using System.Collections;

public class SprintTutoState : TutoState
{

    UnityEngine.UI.Text m_text;
    Character player;
    Vector3 oldPlayerPosition;

    float m_time = 0;
    public float m_duration = 2;

    protected override void Start()
    {
        m_text = GetComponentInChildren<UnityEngine.UI.Text>();

        base.Start();
    }

    public override void enter()
    {
        Debug.Log("Enter SprintTutoState");
        m_ETutoState = ETutoStates.SprintState;
        m_text.text = "To sprint hold [Shift] while you move.";

        GameObject[] goList = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in goList)
        {
            player = go.GetComponent<Character>();
            if (player && player.hasAuthority)
                break;
        }

        oldPlayerPosition = player.transform.position;

        m_pakkuAnimator.SetBool("Sprint", true);

        base.enter();
    }

    public override void update()
    {
        m_time += Time.deltaTime;
        if (m_time >= m_duration && player.m_currentMovementState.m_EState == EStates.SprintingState)
        {
            exit();
        }

        base.update();
    }

    public override void exit()
    {
        m_tutoInfo.m_currentState = m_tutoInfo.m_statePool[(int)ETutoStates.JumpState];
        m_tutoInfo.m_currentState.enter();

        base.exit();
    }
}
