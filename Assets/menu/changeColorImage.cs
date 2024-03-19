using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class changeColorImage : MonoBehaviour {

	public Color[] backgrounds;
	public Color[] texts;

	private Color background;
	private Color textColor;
	private float timeLastUpdate;
	private float wait;

	private Image image;
	private Text text;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image> ();
		text = GetComponentInChildren<Text> ();

		int randColor = Random.Range (0, backgrounds.Length);
		background = backgrounds [randColor];
		textColor = texts [randColor];

		wait = 5f;
		timeLastUpdate = -5f;
	}

	// Update is called once per frame
	void Update () {
		if (Time.time - timeLastUpdate > wait) {
			int randColor = Random.Range (0, backgrounds.Length);
			background = backgrounds [randColor];
			textColor = texts [randColor];

			timeLastUpdate = Time.time;
		} else {
			image.color = Color.Lerp (image.color, background, Time.deltaTime * 2);
			text.color = Color.Lerp (text.color, textColor, Time.deltaTime * 2);
		}
	}
}
