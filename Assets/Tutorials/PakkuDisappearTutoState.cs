using UnityEngine;
using System.Collections;

public class PakkuDisappearTutoState : TutoState
{

    protected override void Start()
    {
        m_pakkuAnimator = GetComponentInChildren<Animator>();

        base.Start();
    }

    public override void enter()
    {
        Debug.Log("Enter PakkuDisappearState");
        m_ETutoState = ETutoStates.PakkuDisappearState;

        m_pakkuAnimator.SetBool("Unpop", true);

        base.enter();
    }
}
