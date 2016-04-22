using UnityEngine;
using System.Collections.Generic;

public class WaterDeviation : MonoBehaviour
{
    public GameObject m_target;
    private List<DeviationEffector> m_effectors = new List<DeviationEffector>();

    void OnTriggerEnter(Collider other)
    {
        Drop drop = other.GetComponent<Drop>();
        if (drop != null)
        {
            drop.removeEffectors();
            DeviationEffector newEffector = drop.gameObject.AddComponent<DeviationEffector>();
            newEffector.init(m_target, 10);
            m_effectors.Add(newEffector);
        }
    }

    void OnTriggerExit(Collider other)
    {

    }
}
