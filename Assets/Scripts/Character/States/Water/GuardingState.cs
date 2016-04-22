using UnityEngine;
using System.Collections;

public class GuardingState : AbleToFallState
{

    public override void enter()
    {
        Debug.Log("Enter GuardingState");

        // ANIMATION
        
        base.enter();
    }

    public override void handleAction(EAction _action)
    {
        switch(_action)
        {
            case EAction.ReleaseGuard:
                print("Release Guard");
                m_character.m_currentActionState = null;
                break;
        }

        base.handleAction(_action);
    }
}
