using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CharacterState : NetworkBehaviour
{
    public EStates m_EState { get; set; }
    protected Character m_character;

    void Start()
    {
        m_character = GetComponent<Character>();
    }

    public virtual void enter() { }

    public virtual void handleAction(EAction _action) { }

    public virtual void handleMovement(EMovement _movement) { }

    public virtual void fixedUpdate() { }

    public virtual void update() { }

    public virtual void exit() { }
}
