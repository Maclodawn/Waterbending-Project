using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ETutoStates
{
    PakkuAppearState,
    GreetingsState,
    PullState,
    TurnAroundState,
    ByeState,
    PakkuDisappearState
}

public class TutoInfo : MonoBehaviour
{

    public List<TutoState> m_statePool { get; set; }
    public TutoState m_currentState { get; set; }

    public float m_startDelay = 0;
    float m_elapsedTime = 0;

    Canvas m_canvas;

    public void init()
    {
        m_statePool = new List<TutoState>();
        m_statePool.Add(GetComponent<PakkuAppearTutoState>());
        m_statePool.Add(GetComponent<GreetingsTutoState>());
        m_statePool.Add(GetComponent<PullTutoState>());
        m_statePool.Add(GetComponent<TurnAroundTutoState>());
        m_statePool.Add(GetComponent<ByeTutoState>());
        m_statePool.Add(GetComponent<PakkuDisappearTutoState>());
        m_canvas = GetComponent<Canvas>();
        m_canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_canvas.enabled)
        {
            m_elapsedTime += Time.deltaTime;
            if (m_elapsedTime >= m_startDelay)
            {
                m_currentState = m_statePool[(int)ETutoStates.PakkuAppearState];
                m_currentState.enter();
                m_canvas.enabled = true;
            }
        }
        else
        {
            m_currentState.update();
        }
    }
}
