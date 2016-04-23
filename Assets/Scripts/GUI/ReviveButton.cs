using UnityEngine;
using System.Collections;

public class ReviveButton : MonoBehaviour
{
    public ComputeActionsFromInput m_character;

	public void Revive()
    {
        m_character.Revive();
    }
}
