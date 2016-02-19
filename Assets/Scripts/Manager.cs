using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{

    private static Manager m_managerInstance = null;
    public static Manager getManager()
    {
        if (m_managerInstance)
            return m_managerInstance;

        m_managerInstance = FindObjectOfType<Manager>();
        return m_managerInstance;
    }

    public Terrain m_terrain;

    [SerializeField]
    GameObject m_originalAI;
    public int nAI;

    [SerializeField]
    GameObject m_originalPlayer;

    [SerializeField]
    GameObject m_mainCamera;

    [SerializeField]
    HealthBarController m_healthBar;

    [SerializeField]
    PowerBarController m_powerBar;

    [SerializeField]
    ChargeBarController m_chargeBar;

    [SerializeField]
    GameObject m_pauseMenu;

    [SerializeField]
    GameObject m_UI;

    public float m_waterGravity;

    bool m_gameIsPaused = false;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameObject player = Instantiate(m_originalPlayer);
        GameObject camera = Instantiate(m_mainCamera);
        PlayerLook playerLook = camera.GetComponentInChildren<PlayerLook>();
        playerLook.m_playerTransform = player.transform;
        player.GetComponent<ComputeActionsFromInput>().m_cameraTransform = camera.transform;

		for (int i = 0; i < nAI; ++i)
			Instantiate(m_originalAI);

        if(m_healthBar != null)
        {
            m_healthBar.Setup(player.GetComponent<HealthComponent>());
        }

        if(m_powerBar != null)
        {
            m_powerBar.Setup(player.GetComponent<PowerComponent>());
        }

        if(m_chargeBar != null)
        {
            m_chargeBar.Setup(player.GetComponent<SpellChargingComponent>());
        }

        if (m_UI != null && m_UI.GetComponent<BloodStain>() != null)
        {
            m_UI.GetComponent<BloodStain>().Setup(player.GetComponent<HealthComponent>());
        }
    }

    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            m_gameIsPaused = !m_gameIsPaused;
            if (!m_gameIsPaused)
                UnPauseGame();
            else
                PauseGame();
        }
    }

    public void OnButtonClicked(string command)
    {
        if(command == "Exit")
        {
            Application.Quit();
        }

        if (command == "ExitToMainMenu")
        {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        m_UI.SetActive(false);
        m_pauseMenu.SetActive(true);

        Time.timeScale = 0;

        Manager.BroadcastAll("ReceiveMessage", "Pause");
    }

    public void UnPauseGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_UI.SetActive(true);

        m_pauseMenu.SetActive(false);

        Time.timeScale = 1;

        Manager.BroadcastAll("ReceiveMessage", "UnPause");
    }

    public static void BroadcastAll(string fun, System.Object msg)
    {
        GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in gos)
        {
            if (go && go.transform.parent == null)
            {
                go.gameObject.BroadcastMessage(fun, msg, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
