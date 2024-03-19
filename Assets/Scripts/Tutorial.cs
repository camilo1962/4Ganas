using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Tutorial : MonoBehaviour {

	public float timeChangeText;
	public string[] stringsTutorial;
	public GameObject textTutorialObject;
	public GameObject textTutorialObject2;

	private bool tutorial;
	private int tutorialCounter;

	private float timeLastUpdate;
	private Text textTutorial;

	// Use this for initialization
	void Start () {
		tutorial = PlayerPrefs.GetInt ("tutorial", 1) == 1 ? true : false;

		defaultState ();
	}
	
	// Update is called once per frame
	void Update () {
		if (tutorial && Time.time - timeLastUpdate > timeChangeText) {
			tutorialCounter++;
			if (tutorialCounter >= stringsTutorial.Length) {
				tutorialCounter = 2;
			}

			loadTutorial (tutorialCounter);
			timeLastUpdate = Time.time;
		}
	}

	private void loadTutorial(int which) {
		textTutorial.text = LocalizationText.GetText ( stringsTutorial [tutorialCounter] );
	}

	public void disable() {
		textTutorialObject2.SetActive (false);
		textTutorialObject.SetActive (false);
	}

	public void defaultState() {
		if (tutorial) {
			textTutorial = textTutorialObject.GetComponent<Text> ();
			textTutorialObject2.SetActive (true);
			textTutorialObject.SetActive (true);

			timeLastUpdate = Time.time;
			tutorialCounter = 0;
		} else {
			textTutorialObject2.SetActive (false);
			textTutorialObject.SetActive (false);
		}
	}
}
