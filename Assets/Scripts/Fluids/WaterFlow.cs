using UnityEngine;
using System.Collections.Generic;

public class WaterFlow : MonoBehaviour {
    public Transform dropPrefab;
    public Vector3 target;
    public float spawnDelay;
    private float time;
    private List<Drop> drops = new List<Drop>();
    public float minRadius, maxRadius;

    public float alpha, beta, speed, deltaAlpha, deltaBeta;


    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        time += Time.deltaTime;

        print(Input.GetAxis("Test"));

        if (Input.GetAxis("Test") > 0)
        {
            alpha += 0.1f;
        }
        else if(Input.GetAxis("Test") < 0)
        {
            alpha -= 0.1f;
        }

        if (time > spawnDelay)
        {
            time -= spawnDelay;
            Drop drop = GameObject.Instantiate<Transform>(dropPrefab).GetComponent<Drop>();
            drop.target = target;
            Vector3 speed = GetRandomSpeed();

            Vector3 AB = target - transform.position;
            Vector3 x = AB.normalized;
            Vector3 z = Vector3.Cross(x, speed).normalized;
            Vector3 y = Vector3.Cross(z, x);
            float vx = Vector3.Project(speed, x).magnitude;
            float vy = Vector3.Project(speed, y).magnitude;
            float dx = Vector3.Project(AB, x).magnitude;
            float dy = Vector3.Project(AB, y).magnitude;

            float g = 2 * vx*vy/AB.magnitude;
            drop.gravity = -y*g;
            drop.speed = speed;
            float radius = Random.value * (maxRadius - minRadius) + minRadius;
            drop.transform.position = transform.position;
            drop.transform.localScale = new Vector3(radius, radius, radius);
        }
	}

    private Vector3 GetRandomSpeed()
    {
        Vector3 AB = target - transform.position;
        Vector3 x = AB.normalized;
        Vector3 z = Vector3.Cross(x, new Vector3(0, 1, 0)).normalized;
        Vector3 y = Vector3.Cross(z, x);
        float randomAlpha = Random.value * deltaAlpha * 2 - deltaAlpha;
        float randomBeta = Random.value * deltaBeta * 2 - deltaBeta;

        return (x * Mathf.Cos(alpha + randomAlpha) + y * Mathf.Sin(alpha + randomAlpha) * Mathf.Cos(beta + randomBeta) + z * Mathf.Sin(alpha + randomAlpha) * Mathf.Sin(beta + randomBeta)) * speed;
    }
}
