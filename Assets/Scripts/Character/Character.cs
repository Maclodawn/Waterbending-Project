using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EStates
{
    //Movement FSM
    StandingState = 0,
    RunningState,
    SprintingState,
    JumpingState,
    JumpDescendingState,
    DodgingState,
    FallingState,
    FallenState,
    //Action FSM
    SelectingWaterToPushState,
    PushingWaterState,
    PullingWaterState,
    //ReleasingWaterControlState,
    TurningWaterAroundState
}

public enum EAction
{
    None,
    SelectWaterToPush,
    PushWater,
    PullWater,
    ReleaseWaterControl,
    TurnWaterAround
}

public enum EMovement
{
    None,
    Run,
    Sprint,
    Jump,
    Dodge,
    Stabilize
}

public class Character : MonoBehaviour
{

    // Final states only
    public List<CharacterState> m_statePool { get; set; }
    public CharacterState m_currentMovementState { get; set; }
    public CharacterState m_currentActionState { get; set; }

    [System.NonSerialized]
    public Animator m_animator;
    public CharacterController m_controller { get; private set; }

    [System.NonSerialized]
    public Vector3 m_velocity; // X: Right, Y: Up, Z: Forward
    [System.NonSerialized]
    public Vector2 m_inputDirection;
    [System.NonSerialized]
    public Vector3 m_movementDirection;
    [System.NonSerialized]
    public Vector3 m_localDirection;
    [System.NonSerialized]
    public float m_currentMoveSpeed;

    public float m_smoothMovement = 1.0f;
    public Vector3 m_gravity = new Vector3(0, -1.0f, 0);

    // Use this for initialization
    void Start()
    {
        m_controller = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();

        m_statePool = new List<CharacterState>();
        // MovementFSM
        m_statePool.Add(GetComponent<StandingState>());
        m_statePool.Add(GetComponent<RunningState>());
        m_statePool.Add(GetComponent<SprintingState>());
        m_statePool.Add(GetComponent<JumpingState>());
        m_statePool.Add(GetComponent<JumpDescendingState>());
        m_statePool.Add(GetComponent<DodgingState>());
        m_statePool.Add(GetComponent<FallingState>());
        m_statePool.Add(GetComponent<FallenState>());
        // Action FSM
        m_statePool.Add(GetComponent<SelectingWaterState>());
        m_statePool.Add(GetComponent<PushingWaterState>());
        m_statePool.Add(GetComponent<PullingWaterState>());
        m_statePool.Add(GetComponent<TurningWaterAroundState>());

        m_currentMovementState = m_statePool[(int)EStates.JumpDescendingState];
        m_currentActionState = null;

        m_currentMovementState.enter(this);
    }

    /*
     * Compute movement/action to do depending on those the character
     * wants to
     */
    public void handleMovementAndAction(EMovement _movement, EAction _action)
    {
        // Actions overwrite movements
        if (m_currentActionState != null)
        {
            m_currentActionState.handleAction(this, _action);
        }
        m_currentMovementState.handleAction(this, _action);
        m_currentMovementState.handleMovement(this, _movement);
    }

    public void FixedUpdate()
    {
        m_currentMovementState.fixedUpdate(this);

        m_movementDirection = transform.forward * m_velocity.z + transform.right * m_velocity.x + transform.up * m_velocity.y;
        m_localDirection = transform.InverseTransformDirection(m_movementDirection);

        m_controller.Move(m_movementDirection * Time.deltaTime);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AbleToFallState toto = (AbleToFallState)m_currentMovementState;
            toto.fall(this);
        }

        // Run movement chosen
        m_currentMovementState.update(this);
        
        // Run action chosen
        if (m_currentActionState != null)
            m_currentActionState.update(this);
    }

    void OnCollisionEnter(Collision collision)
    {
//          Apply Physic
//          SI IL Y A UNE FORCE ASSEZ GRANDE POUR QUE LE PERSONNAGE TOMBE ALORS
//          {
//              if (m_currentMovementState is AbleToFallState
//                  && (m_currentActionState == null || m_currentActionState is AbleToFallState))
//              {
//                  AbleToFallState ableToFallState = (AbleToFallState)m_currentMovementState;
//                  if (ableToFallState != null)
//                  {
//                     ableToFallState.fall(this);
//                  }
//              }
//         }
    }
}
