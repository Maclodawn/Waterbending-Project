using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HealthKit : NetworkBehaviour
{

    private bool m_disabled = false;
    private float m_timer = 0;

    [Header("Health")]
    public float RestoreQuantity;
    public float Cooldown = 5;

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {
        if (!NetworkServer.active)
            return;

        if (m_disabled)
        {
            m_timer += Time.deltaTime;
            if (m_timer >= Cooldown)
                Enable();
        }
        else
        {
            Collider[] colliders = Physics.OverlapBox(transform.position + Vector3.up * 2f, new Vector3(0.75f, 0.75f, 0.25f));
            if (colliders.Length > 0)
                OnTriggerEnter(colliders[0]);
        }
    }

    [ServerCallback]
    void OnTriggerEnter(Collider collider)
    {
        if (!NetworkServer.active)
            return;

        if (m_disabled)
            return;

        HealthComponent health = collider.gameObject.GetComponent<HealthComponent>();
        if (health != null)
        {
            //if(!health.HasMaxHealth())
            {
                health.Health += RestoreQuantity;
                Disable();
            }

            if (health.Health > health.MaxHealth)
                health.Health = health.MaxHealth;
        }
    }

    [Server]
    void Disable()
    {
        RpcDisable();

        Renderer renderer = GetComponent<Renderer>();
        Collider collider = GetComponent<Collider>();

        if (renderer != null)
            renderer.enabled = false;

        if (collider != null)
            collider.enabled = false;

        m_timer = 0;
        m_disabled = true;
    }

    [ClientRpc]
    void RpcDisable()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
            renderer.enabled = false;
    }

    [Server]
    void Enable()
    {
        RpcEnable();

        Renderer renderer = GetComponent<Renderer>();
        Collider collider = GetComponent<Collider>();

        if (renderer != null)
            renderer.enabled = true;

        if (collider != null)
            collider.enabled = true;

        m_timer = 0;
        m_disabled = false;
    }

    [ClientRpc]
    void RpcEnable()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
            renderer.enabled = true;
    }
}
