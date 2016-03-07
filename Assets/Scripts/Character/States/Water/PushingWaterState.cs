using UnityEngine;
using System.Collections;

public class PushingWaterState : AbleToFallState
{

    public float m_speed;
    public Vector3 m_offsetToFling;

    public override void enter(Character _character)
    {
        Debug.Log("Enter PushingWaterState");
        m_EState = EStates.PushingWaterState;

        Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000.0f) && hit.collider.gameObject.tag == "Player")
        {
            _character.m_waterGroup.setTarget(hit.collider.gameObject);
        }
        else
        {
            _character.m_waterGroup.m_target.transform.position = ray.origin + ray.direction * 1000.0f;
        }
        
        Quaternion quaternion = Quaternion.FromToRotation(Vector3.forward, transform.forward);
        Vector3 vect = quaternion * m_offsetToFling;
        _character.m_waterGroup.fling(m_speed, _character.transform.position + _character.m_controller.center + vect);

        base.enter(_character);
    }

    public override void update(Character _character)
    {
        //FIXME
//         if (Animation is Over)
//         {
        _character.m_currentActionState = null;
//         }

        base.update(_character);
    }
}
