using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ComputeActionsFromInput : Character
{

    public GameObject prefabCamera = null;

    private bool respawn = false, hurt = false;

    //To be called from PlayerNetworkSetup
    [Client]
    public void init()
    {
        GameObject camera = Instantiate(prefabCamera);
        PlayerLook playerLook = camera.GetComponentInChildren<PlayerLook>();
        playerLook.m_playerTransform = gameObject.transform;
        playerLook.m_playerTransform.position = playerLook.m_playerTransform.position - playerLook.m_playerTransform.position.y * Vector3.up;
        m_cameraTransform = camera.transform;
    }

    //-----

    public void Respawn()
    {
        respawn = true;
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("Respawn");
        int selected = Random.Range(0, spawns.Length);
        transform.position = spawns[selected].transform.position;
    }

    public void OnDamageTaken()
    {
        hurt = true;
    }

    [System.NonSerialized]
    public Transform m_cameraTransform;

    // Compute movement/action the character wants to do based on inputs
    protected override void Update()
    {
        if (Manager.getInstance().isGamePaused())
            return;
        EMovement movement = EMovement.None;
        EAction action = EAction.None;
        m_inputDirection = new Vector2(Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("LeftAxisX"),
                                        Input.GetAxisRaw("Vertical") - Input.GetAxisRaw("LeftAxisY"));

        if (Input.GetKey(KeyCode.P))
            Debug.Break();

        //Movements
        if (respawn)
        {
            movement = EMovement.Revive;
            respawn = false;
        }
        else if (hurt)
        {
            movement = EMovement.Hurt;
            hurt = false;
        }
        else if (GetComponent<HealthComponent>().Health <= 0)
        {
            movement = EMovement.Die;
        }
        else if (m_currentMovementState is FallingState || m_currentMovementState is FallenState)
        {
            if (Input.GetButtonDown("Stabilize"))
            {
                movement = EMovement.Stabilize;
            }
        }
        else if (m_currentMovementState is AbleToJumpState && Input.GetButtonDown("Jump"))
        {
            movement = EMovement.Jump;
        }
        else if (m_inputDirection.x != 0.0f || m_inputDirection.y != 0.0f)
        {
            if (m_currentMovementState is StandingState || m_currentMovementState is RunningState
                || m_currentMovementState is SprintingState)
            {

                if (Input.GetButtonDown("Dodge"))
                {
                    movement = EMovement.Dodge;
                }
                else if (Input.GetButton("Sprint"))
                {
                    movement = EMovement.Sprint;
                }
                else
                {
                    movement = EMovement.Run;
                }
            }
        }

        //Actions
        if (Input.GetButtonDown("Guard"))
        {
            action = EAction.Counter;
        }
        else if (Input.GetButton("Guard"))
        {
            action = EAction.Guard;
        }
        else if (Input.GetButtonUp("Guard"))
        {
            action = EAction.ReleaseGuard;
        }
        //Press LeftClick + Released RightClick
        else if (getButtonDown("Fire1") && !getButton("Fire2"))
        {
            action = EAction.SelectWaterToPush;
        }
        //Release LeftClick OR Pressed LeftClick + Release RightClick
        else if (getButtonUp("Fire1") || getButton("Fire1") && getButtonUp("Fire2"))
        {
            action = EAction.PushWater;
        }
        //Press RightClick + Released LeftClick
        else if (getButtonDown("Fire2") && !getButton("Fire1"))
        {
            action = EAction.PullWater;
        }
        //Release RightClick
        else if (getButtonUp("Fire2"))
        {
            action = EAction.ReleaseWaterControl;
        }
        //Pressed LeftClick + Pressed RightClick
        else if (getButton("Fire1") && getButton("Fire2"))
        {
            action = EAction.TurnWaterAround;
        }

        handleMovementAndAction(movement, action);

        m_oldFire1AxisRaw = Input.GetAxisRaw("Fire1");
        m_oldFire2AxisRaw = Input.GetAxisRaw("Fire2");

        base.Update();
    }

    float m_oldFire1AxisRaw;
    float m_oldFire2AxisRaw;

    bool getButtonDown(string str)
    {
        if (str.Equals("Fire1"))
        {
            return Input.GetButtonDown(str) || (Input.GetAxisRaw(str) > 0 && m_oldFire1AxisRaw == 0);
        }
        else if (str.Equals("Fire2"))
        {
            return Input.GetButtonDown(str) || (Input.GetAxisRaw(str) > 0 && m_oldFire2AxisRaw == 0);
        }

        return false;
    }

    bool getButton(string str)
    {
        if (str.Equals("Fire1") || str.Equals("Fire2"))
        {
            return Input.GetButton(str) || Input.GetAxisRaw(str) > 0;
        }

        return false;
    }

    bool getButtonUp(string str)
    {
        if (str.Equals("Fire1"))
        {
            return Input.GetButtonUp(str) || (Input.GetAxisRaw(str) == 0 && m_oldFire1AxisRaw > 0);
        }
        else if (str.Equals("Fire2"))
        {
            return Input.GetButtonUp(str) || (Input.GetAxisRaw(str) == 0 && m_oldFire2AxisRaw > 0);
        }

        return false;
    }
}
