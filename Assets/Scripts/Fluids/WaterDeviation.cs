using UnityEngine;
using System.Collections.Generic;

public class WaterDeviation : MonoBehaviour {
    public GameObject target;
    private List<DeviationEffector> effectors = new List<DeviationEffector>();

	void Start () {
	
	}
	
	void FixedUpdate ()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        Drop drop = other.GetComponent<Drop>();
        if (drop != null)
        {
            drop.removeEffectors();
            DeviationEffector newEffector = drop.gameObject.AddComponent<DeviationEffector>();
            newEffector.init(target, 10);
            effectors.Add(newEffector);
        }
    }

    void OnTriggerExit(Collider other)
    {
        
    }
}
