using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class HealthComponent : NetworkBehaviour
{

    [SerializeField]
    private float m_health;

    public float StartingHealth = 100;
    public float MaxHealth = 100;
    public bool CanHaveNegativeHealth = false;
    public bool CanHaveMoreThanMaxHealth = false;

    public delegate void HealthChangedEventHandler(object sender, float _oldHealth, float _newHealth);
    public event HealthChangedEventHandler HealthChanged;

    void Start()
    {
        Health = StartingHealth;
    }

    [ServerCallback]
    void Update()
    {
        if (NetworkServer.active)
        {
            RpcUpdate(m_health);
        }
    }

    [ClientRpc]
    void RpcUpdate(float _health)
    {
        if (_health != m_health)
        {
            ComputeActionsFromInput character = GetComponent<ComputeActionsFromInput>();
            if (character != null && m_health > _health && character.m_currentActionState.m_EState != EStates.GuardingState)
                character.OnDamageTaken();
            m_health = _health;
        }
    }

    public bool HasMaxHealth()
    {
        if (CanHaveMoreThanMaxHealth)
            return false;
        else
            return Health >= MaxHealth;
    }

    public float Health
    {
        get
        {
            return m_health;
        }

        set
        {
            
            float oldHealth = m_health;

            if (value > MaxHealth)
            {
                if (CanHaveMoreThanMaxHealth)
                    m_health = value;
                else
                    m_health = MaxHealth;
            }
            else if (value < 0)
            {
                if (CanHaveNegativeHealth)
                    m_health = value;
                else
                    m_health = 0;
            }
            else
            {
                m_health = value;
            }
            
            ComputeActionsFromInput character = GetComponent<ComputeActionsFromInput>();
            if (character != null && oldHealth > value && character.m_currentActionState.m_EState != EStates.GuardingState)
                character.OnDamageTaken();
            OnHealthChanged(oldHealth, m_health);
        }
    }

    protected void OnHealthChanged(float oldHealth, float newHealth)
    {

        ComputeActionsFromInput character = GetComponent<ComputeActionsFromInput>();
        if (character != null && oldHealth > newHealth && character.m_currentActionState.m_EState != EStates.GuardingState)
            character.OnDamageTaken();
        if (HealthChanged != null)
        {
            HealthChanged(this, oldHealth, newHealth);
            print("HEALTH CHANGED");
        }
    }
}
