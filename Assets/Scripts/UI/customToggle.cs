using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class customToggle : MonoBehaviour {

	public AudioSource audio;

	public string settingsTag;

	public Image background;
	public GameObject toggleElement;

	public bool checkedValue; 
	public Color enabledColor;
	public Color disabledColor;
	public float fadeDuration; //in seconds

	//private Image toggleImage;
	private Color color;
	private float xPosition;

	// Use this for initialization
	void Start () {
		//toggleImage = toggleElement.GetComponent<Image> ();
		checkedValue = PlayerPrefs.GetInt (settingsTag, checkedValue == true ? 1 : 0) == 1 ? true : false;

		setToggle (checkedValue);
	}
	
	// Update is called once per frame
	void Update () {
		background.color = Color.Lerp (background.color, color, Time.deltaTime / fadeDuration);

		float newXPosition = Mathf.Lerp (toggleElement.transform.localPosition.x, xPosition, Time.deltaTime / fadeDuration);

		toggleElement.transform.localPosition = new Vector3 (newXPosition, toggleElement.transform.localPosition.y, toggleElement.transform.localPosition.z);
	}

	private void setToggle(bool enabled) {
		audio.Play ();

		if (enabled) {
			xPosition = -55;
			color = enabledColor;
			PlayerPrefs.SetInt(settingsTag,1);
		} else {
			xPosition = -175;
			color = disabledColor;
			PlayerPrefs.SetInt (settingsTag,0);
		}
	}

	public void toggle() {
		checkedValue = !checkedValue;
		setToggle (checkedValue);
	}
}
