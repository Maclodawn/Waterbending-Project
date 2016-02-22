using UnityEngine;
using System.Collections.Generic;

public class WaterDeviation : MonoBehaviour {
    public GameObject target;
    private List<DropTarget> effectors = new List<DropTarget>();

	void Start () {
	
	}
	
	void FixedUpdate ()
    {
        for(int i = effectors.Count - 1; i >= 0; i--)
        {
            DropTarget effector = effectors[i];
            if(effector.destinationReached)
            {
                GameObject obj = effector.gameObject;
                Destroy(effector);
                DropTarget newEffector = obj.AddComponent<DropTarget>();
                newEffector.init(target);
                effectors.RemoveAt(i);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Drop drop = other.GetComponent<Drop>();
        if (drop != null)
        {
            drop.removeEffectors();
            DropTarget target = drop.gameObject.AddComponent<DropTarget>();
            target.init(gameObject);
            effectors.Add(target);
        }
    }

    void OnTriggerExit(Collider other)
    {
        
    }
}
