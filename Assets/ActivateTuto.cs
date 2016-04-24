using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActivateTuto : MonoBehaviour {

	public Toggle toggle_tuto;

	public void activateTuto() {
		OptionsOnHold optionsHolder = FindObjectOfType<OptionsOnHold>();
		if (optionsHolder)
			optionsHolder.m_tuto = toggle_tuto.isOn;
	}
}
