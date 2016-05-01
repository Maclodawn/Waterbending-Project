using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MenuManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        if (NetworkManager.singleton)
        {
            Destroy(NetworkManager.singleton.gameObject);
        }
    }
}
