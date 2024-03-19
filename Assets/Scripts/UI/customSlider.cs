using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class customSlider : MonoBehaviour {

	public string settingsTag;
	public float defaultValue;

	private Slider slider;

	// Use this for initialization
	void Start () {
		slider = GetComponent<Slider> ();
		setValue(PlayerPrefs.GetFloat (settingsTag, defaultValue));

		slider.onValueChanged.AddListener(setValue);
		setValue(slider.value);
	}

	private void setValue(float value) {
		slider.value = value;
		PlayerPrefs.SetFloat (settingsTag, value);

		AudioListener.volume = value;
	}

}
