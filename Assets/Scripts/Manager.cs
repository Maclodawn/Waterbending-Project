using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Manager : NetworkBehaviour
{
    private static Manager m_managerInstance = null;
    public static Manager getInstance()
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

    List<GameObject> players = new List<GameObject>();
    public void addPlayer(GameObject player)
    {
        players.Add(player);

        if (NetworkClient.active)
        {
            Instantiate(m_tutoPrefab).GetComponent<TutoInfo>().init();
        }
    }

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

    [SerializeField]
    GameObject m_tutoPrefab;

    public GameObject m_waterReservePrefab;
    public GameObject m_waterGroupPrefab;
    public GameObject m_dropPrefab;
    public GameObject m_waterTargetPrefab;
    public GameObject m_dropParticlesPrefab;
    public GameObject m_waterDeflectGuardPrefab;

    public float m_cameraSpeed = 1;
    public bool m_yReversed = true;

    public float m_waterGravity;

    bool m_gameIsPaused = false;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        for (int i = 0; i < nAI; ++i)
            Instantiate(m_originalAI);
    }

    void Update()
    {
        if (NetworkClient.active && Input.GetButtonDown("Pause"))
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
        if (command == "Exit")
        {
            Application.Quit();
        }

        if (command == "ExitToMainMenu")
        {
            //Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }
    }

    public void PauseGame()
    {
        m_gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        m_UI.SetActive(false);
        m_pauseMenu.SetActive(true);

        //Time.timeScale = 0;

        Manager.BroadcastAll("ReceiveMessage", "Pause");
    }

    public void UnPauseGame()
    {
        m_gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_UI.SetActive(true);

        m_pauseMenu.SetActive(false);
        m_pauseMenu.GetComponent<PauseMenu>().OpenMainMenu();

        //Time.timeScale = 1;

        Manager.BroadcastAll("ReceiveMessage", "UnPause");
    }

    public bool isGamePaused()
    {
        return m_gameIsPaused;
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
