using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyScript : MonoBehaviour {

	public AudioSource audio;

	public Image star2;
	public Image star3;

	public Sprite filledStar;
	public Sprite unfilledStar;

	private int starsSelected;

	// Use this for initialization
	void Start () {
		starsSelected = 1;

		updateStars ();
		updateAttribute ();
	}
	
	public void clickedDifficulty() {
		audio.Play ();

		starsSelected++;

		if (starsSelected > 3) {
			starsSelected = 1;
		}

		updateStars ();
		updateAttribute ();
	}

	private void updateStars() {
		if (starsSelected == 1) {
			star2.sprite = unfilledStar;
			star3.sprite = unfilledStar;
		} else if (starsSelected == 2) {
			star2.sprite = filledStar;
			star3.sprite = unfilledStar;
		} else if (starsSelected == 3) {
			star2.sprite = filledStar;
			star3.sprite = filledStar;
		}
	}

	private void updateAttribute() {
		MenuAttributes.difficulty = starsSelected;
	}
}
