using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LaunchGame : MonoBehaviour
{

    public GameObject main_buttons;
    public OptionsOnHold options_holder;

    public void launchGameWithTuto()
    {
        main_buttons.SetActive(false);
        options_holder.m_tuto = true;
        SceneManager.LoadScene("BelkaLobby");
    }

    public void launchGameWithoutTuto()
    {
        main_buttons.SetActive(false);
        options_holder.m_tuto = false;
        SceneManager.LoadScene("BelkaLobby");
    }
}
