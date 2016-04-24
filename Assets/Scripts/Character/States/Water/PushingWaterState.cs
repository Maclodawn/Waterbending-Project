using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PushingWaterState : AbleToFallState
{
    public float m_speed;
    Vector3 m_offsetToFling;
    float m_alpha;
    bool m_fromTurn = false;
    [SyncVar]
    bool isReady = false;

    float time = 0.0f;
    public float cooldown;

	private PowerComponent power_component = null;

    [Client]
    public void init(Vector3 _offsetToFling, float _alpha, bool _fromTurn)
    {
        time = 0.0f;
        m_offsetToFling = _offsetToFling;
        m_alpha = _alpha;
        m_fromTurn = _fromTurn;
        CmdInit(m_offsetToFling, m_alpha, m_fromTurn, m_speed);
    }

    [Command]
    void CmdInit(Vector3 _offsetToFling, float _alpha, bool _fromTurn, float _speed)
    {
        m_offsetToFling = _offsetToFling;
        m_alpha = _alpha;
        m_fromTurn = _fromTurn;
        m_speed = _speed;
    }

    [Client]
    public override void enter()
    {
        Debug.Log("Enter PushingWaterState");
        m_EState = EStates.PushingWaterState;
        GetComponent<Animator>().SetBool("Push", true);

        GameObject newTarget = AutoAim();
        if (newTarget)
        {
            CmdEnterFound(newTarget.GetComponent<NetworkIdentity>());
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
            CmdEnterNew(ray.origin, ray.direction);
        }

        base.enter();
    }

    [Command]
    void CmdEnterFound(NetworkIdentity _targetIdentity)
    {
        GameObject newTarget = _targetIdentity.gameObject;

        enter(newTarget);
    }

    [Command]
    void CmdEnterNew(Vector3 _origin, Vector3 _direction)
    {
        GameObject newTarget = GameObject.Instantiate(Manager.getInstance().m_waterTargetPrefab);
        newTarget.transform.position = _origin + _direction * 40.0f;
        NetworkServer.SpawnWithClientAuthority(newTarget, gameObject);

        enter(newTarget);
    }

    [Server]
    void enter(GameObject newTarget)
    {
        Vector3 dir = newTarget.transform.position - m_character.transform.position;
        Quaternion quaternion = Quaternion.FromToRotation(Vector3.forward, new Vector3(dir.x, 0, dir.z));
        m_offsetToFling = quaternion * m_offsetToFling;

        isReady = true;

        m_character.m_waterGroup.setTarget(newTarget);

		//updating power of water group and my power
		if (power_component == null)
			power_component = m_character.GetComponent<PowerComponent>();
		m_character.m_waterGroup.power_percent = power_component.Power/power_component.MaxPower;
		power_component.Power = power_component.Power/2f;

        if (m_fromTurn)
            m_character.m_waterGroup.flingFromTurn(m_speed, m_character.transform.position + m_character.m_controller.center + m_offsetToFling, m_alpha);
        else
            m_character.m_waterGroup.flingFromSelect(m_speed, m_character.transform.position + m_character.m_controller.center + m_offsetToFling, m_alpha);
    }

    [Client]
    public override void update()
    {
        if (!isReady)
            return;

        if (!m_character.m_waterGroup.m_flingingFromSelect && !m_character.m_waterGroup.m_flingingFromTurn && time > cooldown)
        {
            exit();
        }

        time += Time.deltaTime;
        base.update();
    }

    public override void exit()
    {
        m_character.m_currentActionState = null;
        GetComponent<Animator>().SetBool("Push", false);

        base.exit();
    }

    [Client]
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
