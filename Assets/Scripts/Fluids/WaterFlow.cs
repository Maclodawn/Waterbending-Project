using UnityEngine;
using System.Collections.Generic;

public class WaterFlow : MonoBehaviour
{
    public Transform dropPrefab;
    public float spawnDelay;
    private float time;
    public float minRadius, maxRadius;

    public float alpha, beta, speed, deltaAlpha, deltaBeta;

    public GameObject target;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;


        if (Input.GetAxis("Test") > 0)
        {
            alpha += 0.1f;
        }
        else if (Input.GetAxis("Test") < 0)
        {
            alpha -= 0.1f;
        }

        if (time > spawnDelay)
        {
            time -= spawnDelay;
            Drop drop = GameObject.Instantiate<Transform>(dropPrefab).GetComponent<Drop>();
            Vector3 speed = GetRandomSpeed();
            drop.transform.position = transform.position;
            //drop.SetTarget(target.transform.position, speed);
            //drop.SetSpeed(speed);
            drop.initVelocity(speed);
            float radius = Random.value * (maxRadius - minRadius) + minRadius;
            drop.transform.localScale = new Vector3(radius, radius, radius);
            drop.gameObject.AddComponent<DropTarget>();
            drop.GetComponent<DropTarget>().init(target, speed);
        }
    }

    private Vector3 GetRandomSpeed()
    {
        Vector3 AB = target.transform.position - transform.position;
        Vector3 x = AB.normalized;
        Vector3 z = Vector3.Cross(x, new Vector3(0, 1, 0)).normalized;
        Vector3 y = Vector3.Cross(z, x);
        float randomAlpha = Random.value * deltaAlpha * 2 - deltaAlpha;
        float randomBeta = Random.value * deltaBeta * 2 - deltaBeta;

        return (x * Mathf.Cos(alpha + randomAlpha) + y * Mathf.Sin(alpha + randomAlpha) * Mathf.Cos(beta + randomBeta) + z * Mathf.Sin(alpha + randomAlpha) * Mathf.Sin(beta + randomBeta)) * speed;
    }
}
