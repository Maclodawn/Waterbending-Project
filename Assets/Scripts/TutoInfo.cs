using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ETutoStates
{
    PakkuAppearState,
    GreetingsState,
    PullState,
    ByeState,
    PakkuDisappearState
}

public class TutoInfo : MonoBehaviour
{

    public List<TutoState> m_statePool { get; set; }
    public TutoState m_currentState { get; set; }

    // Use this for initializations
    void Start()
    {
        m_statePool = new List<TutoState>();
        m_statePool.Add(GetComponent<PakkuAppearTutoState>());
        m_statePool.Add(GetComponent<GreetingsTutoState>());
        m_statePool.Add(GetComponent<PullTutoState>());
        m_statePool.Add(GetComponent<ByeTutoState>());
        m_statePool.Add(GetComponent<PakkuDisappearTutoState>());

        m_currentState = m_statePool[(int)ETutoStates.PakkuAppearState];
        m_currentState.enter();
    }

    // Update is called once per frame
    void Update()
    {
        m_currentState.update();
    }
}
