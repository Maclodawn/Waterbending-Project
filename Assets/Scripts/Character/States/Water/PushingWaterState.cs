using UnityEngine;
using System.Collections;

public class PushingWaterState : AbleToFallState
{

    public float m_speed;
    Vector3 m_offsetToFling;
    float m_alpha;
    bool m_fromTurn = false;

    public void init(Vector3 _offsetToFling, float _alpha, bool _fromTurn)
    {
        m_offsetToFling = _offsetToFling;
        m_alpha = _alpha;
        m_fromTurn = _fromTurn;
    }

    public override void enter(Character _character)
    {
        Debug.Log("Enter PushingWaterState");
        m_EState = EStates.PushingWaterState;

        GameObject newTarget = AutoAim();
        if (!newTarget)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
            newTarget = new GameObject();
            newTarget.name = "WaterTarget";
            newTarget.transform.position = ray.origin + ray.direction * 40.0f;
        }

        _character.m_waterGroup.setTarget(newTarget);
        if (m_fromTurn)
            _character.m_waterGroup.flingFromTurn(m_speed, _character.transform.position + _character.m_controller.center + m_offsetToFling, m_alpha);
        else
            _character.m_waterGroup.flingFromSelect(m_speed, _character.transform.position + _character.m_controller.center + m_offsetToFling, m_alpha);

        base.enter(_character);
    }

    public override void update(Character _character)
    {
        if (!_character.m_waterGroup.m_flingingFromSelect && !_character.m_waterGroup.m_flingingFromTurn)
        {
            //FIXME
            //         if (Animation is Over)
            //         {
            _character.m_currentActionState = null;
            //         }
        }

        base.update(_character);
    }

    GameObject AutoAim()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closestplayer = null;
        float closestPlayerDistance = float.MaxValue;

        foreach (GameObject player in players)
        {
            if (player == gameObject)
                continue;

            Vector3 point = Camera.main.WorldToViewportPoint(player.transform.position);
            if (point.z < 0 || point.x < 0 || point.x > 1 || point.y < 0 || point.y > 1)
                continue;

            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (closestPlayerDistance > dist)
            {
                closestplayer = player;
                closestPlayerDistance = dist;
            }
        }

        return closestplayer;
    }
}
