using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;

public class LaunchGame : MonoBehaviour
{

    public GameObject main_buttons;
    public OptionsOnHold options_holder;

//     void Start()
//     {
//         NetworkManager manager = NetworkManager.singleton;
//         if (manager)
//             manager.de
//     }

    public void launchGame()
    {
//         NetworkManager manager = NetworkManager.singleton;
//         if (manager)
//         {
//             manager.StopHost();
//             print("Stopped host");
//         }

        options_holder.m_tuto = false;
        main_buttons.SetActive(false);
        Scene scene = SceneManager.GetSceneByName("BelkaLobby");
        if (!scene.isLoaded)
            SceneManager.LoadScene("BelkaLobby");
        else
            SceneManager.SetActiveScene(scene);

        NetworkManager manager = NetworkManager.singleton;
        if (manager)
        {
            manager.StopHost();
            print("Stopped host");
        }
    }
}
