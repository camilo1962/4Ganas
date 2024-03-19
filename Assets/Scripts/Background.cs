using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

	public float fadeDuration = 0.5f; //in seconds

	public Color playerOneBackground;
	public Color playerTwoBackground;

	private SpriteRenderer spriteRenderer;
	private Color color;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		color = playerOneBackground;
	}

	void Update() {
		spriteRenderer.color = Color.Lerp (spriteRenderer.color, color, Time.deltaTime / fadeDuration);
	}

	public void setBackground(bool playerOne) {
		if (playerOne) {
			color = playerOneBackground;
		} else {
			color = playerTwoBackground;
		}
	}
}
