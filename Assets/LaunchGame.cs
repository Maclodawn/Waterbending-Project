using UnityEngine;
using System.Collections;

public class LaunchGame : MonoBehaviour {

	public GameObject main_buttons;
	public OptionsOnHold options_holder;

	public void launchGameWithTuto() {
		main_buttons.SetActive(false);
		options_holder.m_tuto = true;
		Application.LoadLevel("BelkaLobby");
	}

	public void launchGameWithoutTuto() {
		main_buttons.SetActive(false);
		options_holder.m_tuto = false;
		Application.LoadLevel("BelkaLobby");
	}
}
