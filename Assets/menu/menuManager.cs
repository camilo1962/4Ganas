using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuManager : MonoBehaviour {

	public AudioSource tickSound;

	public GameObject menuCanvas;
	public GameObject settingsCanvas;
	public GameObject singleplayerCanvas;
	public GameObject localMulitplayerCanvas;

	public InputField firstPlayer;
	public InputField secondPlayer;

	void Start() {
		//Load audio settings
		float value = PlayerPrefs.GetFloat ("sound", 1);
		AudioListener.volume = value;

		//load language settings
		//german or english is supported for now
		SystemLanguage lang = Application.systemLanguage; 
		if (lang == SystemLanguage.German) {
			LocalizationText.SetLanguage ("DE");
		} else {
			LocalizationText.SetLanguage ("EN");
		}

		menuCanvas.SetActive (true);
		settingsCanvas.SetActive (false);
		singleplayerCanvas.SetActive (false);
		localMulitplayerCanvas.SetActive (false);
	}

	public void startSingleplayer() {
		tickSound.Play ();

		MenuAttributes.vsKi = true;
		//MenuAttributes.difficulty = 1; is already set in the DifficultyScript
		MenuAttributes.firstPlayerName = LocalizationText.GetText ("you");

		SceneManager.LoadScene ("singleplayer");
	}

	public void startLocalMultiplayer() {
		tickSound.Play ();

		MenuAttributes.vsKi = false;
		MenuAttributes.firstPlayerName = firstPlayer.text;
		MenuAttributes.secondPlayerName = secondPlayer.text;

		SceneManager.LoadScene ("singleplayer");
	}

	public void openSingleplayerMenu() {
		tickSound.Play ();

		menuCanvas.SetActive (false);
		settingsCanvas.SetActive (false);
		singleplayerCanvas.SetActive (true);
		localMulitplayerCanvas.SetActive (false);
	}

	public void openSettings() {
		tickSound.Play ();

		menuCanvas.SetActive (false);
		settingsCanvas.SetActive (true);
		singleplayerCanvas.SetActive (false);
		localMulitplayerCanvas.SetActive (false);
	}

	public void openMenu() {
		tickSound.Play ();

		menuCanvas.SetActive (true);
		settingsCanvas.SetActive (false);
		singleplayerCanvas.SetActive (false);
		localMulitplayerCanvas.SetActive (false);
	}

	public void openLocalMultiplayerMenu() {
		tickSound.Play ();

		menuCanvas.SetActive (false);
		settingsCanvas.SetActive (false);
		singleplayerCanvas.SetActive (false);
		localMulitplayerCanvas.SetActive (true);
	}
}
