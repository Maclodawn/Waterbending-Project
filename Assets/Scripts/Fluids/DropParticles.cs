using UnityEngine;
using System.Collections;

public class DropParticles : MonoBehaviour
{
    public Drop drop;
    public ParticleSystem particles;

	void Start ()
    {
        particles = GetComponent<ParticleSystem>();
	}
	
	void Update ()
    {
        if (drop != null)
            transform.position = drop.transform.position;
        else
        {
            particles.Stop();
            if (!particles.IsAlive())
                Destroy(gameObject);
        }
	}

    void OnTriggerEnter(Collider other)
    {
    }
}
