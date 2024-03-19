using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class calculating : MonoBehaviour {

	private Text text;
	public float timeIntervall = 0.5f;

	private int counter;
	private int maxCounter = 7;
	private float lastUpdate;

	private string original;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		lastUpdate = Time.time;

		original = LocalizationText.GetText (text.text);
		text.text = original;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - lastUpdate > timeIntervall) {
			lastUpdate = Time.time;
			text.text = text.text + ".";

			counter++;
			if (counter > maxCounter) {
				counter = 0;
				text.text = original;
			}
		}
	}
}
