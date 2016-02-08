using UnityEngine;
using System.Collections;

public class Drop : MonoBehaviour {
    public Vector3 target;
    public Vector3 speed;
    public Vector3 gravity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        speed += gravity * Time.deltaTime;
        transform.position += speed * Time.deltaTime;
	}
}
