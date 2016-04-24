using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CounteringWaterState : AbleToFallState
{

    public float m_upperDistanceToCounter = 5.0f;
    public float m_lowerDistanceToCounter = 5.0f;
    public float m_angleDirectionToCounter = 45.0f;
    public float m_radiusToDeviate = 2.0f;

//     [Client]
//     void OnDrawGizmos()
//     {
//         Gizmos.DrawSphere(transform.position + m_character.m_controller.center, m_upperDistanceToCounter);
//         Gizmos.DrawSphere(transform.position + m_character.m_controller.center, m_lowerDistanceToCounter);
//     }

    [Client]
    public override void enter()
    {
        Debug.Log("Enter CounteringWaterState");

        m_EState = EStates.CounteringWaterState;

        Drop drop = getNearestWaterGroupInCharacterDirection();
        if (drop)
        {
            if (Vector3.Distance(drop.transform.position, transform.position + m_character.m_controller.center) > m_lowerDistanceToCounter)
            {
                // TurnAround -> Push
                m_character.m_currentActionState = m_character.m_statePool[(int)EStates.TurningWaterAroundState];
                m_character.m_waterGroup = drop.m_waterGroup;
                (m_character.m_currentActionState as TurningWaterAroundState).initCounter();
                m_character.m_currentActionState.enter();

                CounterTutoState tuto = GameObject.FindObjectOfType<CounterTutoState>();
                if (tuto)
                    tuto.m_countered = true;
            }
            else
            {
                // Deviate
                CmdDeviate(drop.GetComponent<NetworkIdentity>());
                m_character.m_currentActionState = null;
                DeviateTutoState tuto = GameObject.FindObjectOfType<DeviateTutoState>();
                if (tuto)
                    tuto.m_deviated = true;
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
        if (_dropIdentity.GetComponent<Drop>().m_waterGroup == null)
            exit();
        else _dropIdentity.GetComponent<Drop>().m_waterGroup.deviate(m_radiusToDeviate);
    }

    Drop getNearestWaterGroupInCharacterDirection()
    {
        Vector3 characterPosition = transform.position + m_character.m_controller.center;
        Collider[] drops = Physics.OverlapSphere(characterPosition, m_upperDistanceToCounter,
                                                 1 << LayerMask.NameToLayer("Water"), QueryTriggerInteraction.Collide);

        Drop closestDrop = null;
        float closestDropDistance = 0;
        for (int i = 0; i < drops.Length; ++i)
        {
            Drop drop = drops[i].GetComponent<Drop>();
            //Direction test
            if (Mathf.Abs(Vector3.Angle(drop.velocity, characterPosition - drop.transform.position)) < m_angleDirectionToCounter)
            {
                //Distance test
                if (!closestDrop || closestDropDistance > Vector3.Distance(drop.transform.position, characterPosition))
                {
                    closestDrop = drop;
                    closestDropDistance = Vector3.Distance(closestDrop.transform.position, characterPosition);
                }
            }
        }

        return closestDrop;
    }
}
