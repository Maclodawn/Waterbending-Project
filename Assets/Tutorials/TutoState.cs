using UnityEngine;
using System.Collections;

public class TutoState : MonoBehaviour
{
    protected TutoInfo m_tutoInfo;
    protected Animator m_pakkuAnimator;

    public ETutoStates m_ETutoState { get; set; }

    protected virtual void Start()
    {
        m_tutoInfo = GetComponent<TutoInfo>();
        m_pakkuAnimator = GetComponentInChildren<Animator>();
    }

    public virtual void enter() { }

    public virtual void update() { }

    public virtual void exit() { }
}
