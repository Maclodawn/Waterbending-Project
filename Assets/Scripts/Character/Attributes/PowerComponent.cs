using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PowerComponent : NetworkBehaviour
{

    private float m_power;

    [Header("Default")]
    public float StartingPower = 100;
    public float MaxPower = 100;
    public bool CanHaveNegativePower = false;
    public bool CanHaveMoreThanMaxPower = false;

    [Header("Regeneration")]
    public float RegenerationRate = 0;

    public delegate void PowerChangedEventHandler(object sender, float _newPower);
    public event PowerChangedEventHandler PowerChanged;

    void Start()
    {
        Power = StartingPower;
    }

    void Update()
    {
        if (RegenerationRate > 0)
        {
            Power += RegenerationRate * Time.deltaTime;
        }
    }

    public bool HasMaxPower()
    {
        return Power >= MaxPower;
    }

    public float Power
    {
        get
        {
            return m_power;
        }

        set
        {
            if (m_power != value)
            {
                if (value > MaxPower)
                {
                    if (CanHaveMoreThanMaxPower)
                        m_power = value;
                    else if (m_power == MaxPower)
                        return;
                    else
                        m_power = MaxPower;
                }
                else if (value < 0)
                {
                    if (CanHaveNegativePower)
                        m_power = value;
                    else if (m_power == 0)
                        return;
                    else
                        m_power = 0;
                }
                else
                {
                    m_power = value;
                }

                OnPowerChanged(m_power);
            }
        }
    }

    public bool CanLaunchAttack(float cost)
    {
        return cost <= Power;
    }

    protected void OnPowerChanged(float newPower)
    {
        if (NetworkServer.active)
            RpcUpdatePower(newPower);

        if (PowerChanged != null)
            PowerChanged(this, newPower);
    }

    [ClientRpc]
    void RpcUpdatePower(float power)
    {
        Power = power;
    }
}
