using UnityEngine;
using System.Collections;

public class PushingWaterState : AbleToFallState
{

    public override void enter(Character _character)
    {
        Debug.Log("Enter PushingWaterState");
        m_EState = EStates.PushingWaterState;

        base.enter(_character);
    }

    public override void update(Character _character)
    {
        //FIXME
//         if (Animation is Over)
//         {
//             _character.m_currentActionState = null;
//         }

        base.update(_character);
    }
}
