using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CounteringWaterState : AbleToFallState
{

    public float m_upperDistanceToCounter = 5.0f;
    public float m_lowerDistanceToCounter = 4.0f;
    public float m_angleDirectionToCounter = 45.0f;
    public float m_radiusToDeviate = 1.0f;

    [Client]
    public override void enter()
    {
        Debug.Log("Enter CounteringWaterState");

        Drop drop = getNearestWaterGroupInCharacterDirection();
        if (drop)
        {
            if (Vector3.Distance(drop.transform.position, transform.position) > m_lowerDistanceToCounter)
            {
                // TurnAround -> Push
                m_character.m_currentActionState = m_character.m_statePool[(int)EStates.TurningWaterAroundState];
                m_character.m_waterGroup = drop.m_waterGroup;
                (m_character.m_currentActionState as TurningWaterAroundState).initCounter();
                m_character.m_currentActionState.enter();
            }
            else
            {
                // Deviate
                CmdDeviate(drop.GetComponent<NetworkIdentity>());
                m_character.m_currentActionState = null;
            }
        }
        else
        {
            m_character.m_currentActionState = m_character.m_statePool[(int)EStates.GuardingState];
            m_character.m_currentActionState.enter();
        }

        base.enter();
    }

    [Command]
    void CmdDeviate(NetworkIdentity _dropIdentity)
    {
        _dropIdentity.GetComponent<Drop>().m_waterGroup.deviate(m_radiusToDeviate);
    }

    Drop getNearestWaterGroupInCharacterDirection()
    {
        Collider[] drops = Physics.OverlapSphere(transform.position, m_upperDistanceToCounter,
                                                 1 << LayerMask.NameToLayer("Water"), QueryTriggerInteraction.Collide);

        Drop closestDrop = null;
        float closestDropDistance = 0;
        for (int i = 0; i < drops.Length; ++i)
        {
            Drop drop = drops[i].GetComponent<Drop>();
            //Direction test
            if (Mathf.Abs(Vector3.Angle(drop.velocity, transform.position - drop.transform.position)) < m_angleDirectionToCounter)
            {
                //Distance test
                if (!closestDrop || closestDropDistance > Vector3.Distance(drop.transform.position, transform.position))
                {
                    closestDrop = drop;
                    closestDropDistance = Vector3.Distance(closestDrop.transform.position, transform.position);
                }
            }
        }

        return closestDrop;
    }
}
