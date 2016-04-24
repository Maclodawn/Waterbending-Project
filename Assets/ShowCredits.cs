using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowCredits : MonoBehaviour {

	public GameObject credits;
	public GameObject main_buttons;

	public void showCredits() {
		main_buttons.SetActive(false);
		credits.SetActive(true);
	}

	public void showMainButtons() {
		main_buttons.SetActive(true);
		credits.SetActive(false);
	}
}
