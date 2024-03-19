using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColorRenderer : MonoBehaviour {

	private Color color;
	private float timeLastUpdate;
	private float wait;

	private Renderer rend;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
		color = Random.ColorHSV ();

		wait = 0f;
		timeLastUpdate = 0f;
	}

	// Update is called once per frame
	void Update () {
		if (Time.time - timeLastUpdate > wait) {
			color = Random.ColorHSV ();
			wait = Random.Range (1000, 5000) / 1000f;

			timeLastUpdate = Time.time;
		} else {
			rend.material.color = Color.Lerp (rend.material.color, color, Time.deltaTime / wait); 
		}
	}
}
