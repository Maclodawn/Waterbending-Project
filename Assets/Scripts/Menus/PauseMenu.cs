using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject mainMenu, optionsMenu;
    public Slider sensitivitySlider;
    public Toggle reverseYToggle;

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

    public void SetSensitivity()
    {
        Manager.getManager().m_cameraSpeed = sensitivitySlider.value;
    }

    public void SetYCamDir()
    {
        Manager.getManager().m_yReversed = reverseYToggle.isOn;
    }
}
