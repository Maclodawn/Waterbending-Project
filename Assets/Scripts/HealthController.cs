using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;

public class HealthController : NetworkBehaviour
{

    private HealthComponent health = null;
    private InformationsLog informations = null;

    public void Start()
    {
        //retrieving health component
        health = gameObject.GetComponent<HealthComponent>();
        if (health == null)
            throw new ArgumentException("Health Component not found.");

        //retrieving InformationsLog
        informations = GameObject.Find("InformationsLog").GetComponent<InformationsLog>();
    }

    [ServerCallback]
    public void OnTriggerEnter(Collider collider)
    {
        applyDamage(collider);
    }
    
    //when collision, apply damages to player's health component
    [Server]
    public void applyDamage(Collider collider)
    {
        if (collider.gameObject.tag.Contains("Drop"))
        {
            float tmp_dmg = UnityEngine.Random.Range(10, 15);
            health.Health -= tmp_dmg; //TODO way of computing damage=f(power)?
            informations.log("<b><color=\"blue\">" + gameObject.name + "</color></b>: -" + tmp_dmg + "PV");
            if (health.Health < 0.1f)
            {
                //you're dead if your current player is dead
                informations.log("<b><color=\"red\">" + gameObject.name + "</color></b> IS DEAD");
                
            }
        }
    }
}
