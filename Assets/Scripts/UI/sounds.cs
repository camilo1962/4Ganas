using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sounds : MonoBehaviour {

	public AudioSource tickAudio;
	public AudioSource winnerAudio;
	public AudioSource looserAudio;

	public void playTick() {
		tickAudio.Play();
	}

	public void playWinner() {
		winnerAudio.Play ();
	}

	public void playLooser() {
		looserAudio.Play ();
	}
}
