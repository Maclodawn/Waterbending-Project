using UnityEngine;
using System.Collections;

public class ComputeActionsFromInput : Character
{
    Manager mgr = null;

    public GameObject prefabCamera = null;

    //To be called from PlayerNetworkSetup
    public void init()
    {
        mgr.addPlayer(gameObject);
        GameObject camera = Instantiate(prefabCamera);
        PlayerLook playerLook = camera.GetComponentInChildren<PlayerLook>();
        playerLook.m_playerTransform = gameObject.transform;
        playerLook.m_playerTransform.position = playerLook.m_playerTransform.position - playerLook.m_playerTransform.position.y * Vector3.up;
        m_cameraTransform = camera.transform;
    }

    public void Awake()
    {
        mgr = Manager.getInstance();
    }

    //-----

    [System.NonSerialized]
    public Transform m_cameraTransform;

    // Compute movement/action the character wants to do based on inputs
    protected override void Update()
    {
        EMovement movement = EMovement.None;
        EAction action = EAction.None;

        m_inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"),
                                        Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(KeyCode.P))
            Debug.Break();

        //Movements
        if (m_currentMovementState is FallingState || m_currentMovementState is FallenState)
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
        if (Input.GetButton("Fire1") && !Input.GetButton("Fire2"))
        {
            action = EAction.SelectWaterToPush;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            action = EAction.PushWater;
        }
        else if (Input.GetButtonDown("Fire2") && !Input.GetButton("Fire1"))
        {
            action = EAction.PullWater;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            action = EAction.ReleaseWaterControl;
        }
        else if (Input.GetButton("Fire1") && Input.GetButton("Fire2"))
        {
            action = EAction.TurnWaterAround;
        }

        handleMovementAndAction(movement, action);

        base.Update();
    }
}
