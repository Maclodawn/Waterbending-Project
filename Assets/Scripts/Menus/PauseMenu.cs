using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GameObject mainMenu, optionsMenu;

    public void Resume()
    {
        Manager.getManager().UnPauseGame();
    }

    public void OpenOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void OpenMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
}
