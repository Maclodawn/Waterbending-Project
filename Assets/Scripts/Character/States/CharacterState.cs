using UnityEngine;
using System.Collections;

public class CharacterState : MonoBehaviour
{
    public EStates m_EState { get; set; }

    public virtual void enter(Character _character) { }

    public virtual void handleAction(Character _character, EAction _action) { }

    public virtual void handleMovement(Character _character, EMovement _movement) { }

    public virtual void fixedUpdate(Character _character) { }

    public virtual void update(Character _character) { }

    public virtual void exit(Character _character) { }

    public virtual void cancel(Character _character) { }
}
