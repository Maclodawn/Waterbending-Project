using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowOptions : MonoBehaviour {

	public GameObject options;
	public GameObject main_buttons;
	public Slider sensitivitySlider;
	public Toggle reverseYToggle;

	public void showOptions() {
		main_buttons.SetActive(false);
		options.SetActive(true);
	}

	public void showMainButtons() {
		main_buttons.SetActive(true);
		options.SetActive(false);
	}

	public void SetSensitivity() {
		Manager.getInstance().m_cameraSpeed = sensitivitySlider.value;
	}

	public void SetYCamDir()
	{
		Manager.getInstance().m_yReversed = reverseYToggle.isOn;
	}
}
