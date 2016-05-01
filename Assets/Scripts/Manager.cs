﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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

    public Dictionary<int, List<NetworkIdentity>> m_teams = new Dictionary<int, List<NetworkIdentity>>();
    public void addPlayer(GameObject player)
    {
        NetworkIdentity playerId = player.GetComponent<NetworkIdentity>();
        List<NetworkIdentity> team;
        Teammate teammate = player.GetComponent<Teammate>();
        if (!m_teams.TryGetValue(teammate.m_teamId, out team))
        {
            team = new List<NetworkIdentity>();
            team.Add(playerId);
            m_teams.Add(teammate.m_teamId, team);
        }
        //else
        //FIXME
    }

    public void startTuto()
    {
        if (m_tuto && NetworkClient.active)
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

    [SerializeField]
    GameObject m_deathMenu;

    public GameObject m_winnerMenu;

    public GameObject m_waterReservePrefab;
    public GameObject m_waterGroupPrefab;
    public GameObject m_dropPrefab;
    public GameObject m_waterTargetPrefab;
    public GameObject m_dropParticlesPrefab;
    public GameObject m_waterDeflectGuardPrefab;

    public float m_cameraSpeed = 1;
    public bool m_yReversed = true;

    public float m_waterGravity;

    [SerializeField]
    bool m_tuto = false;

    bool m_gameIsPaused = false;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        for (int i = 0; i < nAI; ++i)
            Instantiate(m_originalAI);
    }

    public void Start()
    {
        NetworkManager networkManager = NetworkManager.singleton;
        networkManager.dontDestroyOnLoad = false;

        OptionsOnHold options = FindObjectOfType<OptionsOnHold>();
        if (options)
        {
            m_cameraSpeed = options.m_cameraSpeed;
            m_yReversed = options.m_yReversed;
            m_tuto = options.m_tuto;
        }
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
            NetworkManager networkManager = NetworkManager.singleton;
            if (networkManager)
            {
                if (Network.isClient)
                {
                    if (Network.isServer)
                        networkManager.StopHost();
                    else
                        networkManager.StopClient();
                }
                else
                    networkManager.StopServer();

                Destroy(networkManager.GetComponent<NetworkManagerHUD>());
            }

            Destroy(FindObjectOfType<OptionsOnHold>().gameObject);

//             Scene scene = SceneManager.GetSceneByName("MenuPrincipal");
//             if (scene.isLoaded)
//                 SceneManager.SetActiveScene(scene);
//             else
            SceneManager.LoadScene("MenuPrincipal");
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

    private ComputeActionsFromInput currentCharacter;

    public void ShowDeathMenu(ComputeActionsFromInput character)
    {
        currentCharacter = character;
        m_deathMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Respawn()
    {
        print("Respawn");
        m_deathMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentCharacter.GetComponent<HealthComponent>().Health = currentCharacter.GetComponent<HealthComponent>().StartingHealth;
        currentCharacter.Respawn();
    }
}
