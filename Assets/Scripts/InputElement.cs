using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputElement : MonoBehaviour {

	private float fadeDuration = 0.3f; //in seconds

	public Color defaultColor;
	private Color color;
	public Color hover;
	public Color lastInsert;

	private int position;
	private Color actualColor;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		color = defaultColor;

		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.color = color;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		actualColor = color;
	}

	void LateUpdate() {
		spriteRenderer.color = Color.Lerp (spriteRenderer.color, actualColor, Time.deltaTime / fadeDuration);
	}

	public void onHover() {
		actualColor = hover;
	}

	public void inserted() {
		color = lastInsert;
	}

	public void resetColor() {
		color = defaultColor;
	}

	public void setPosition(int position) {
		this.position = position;
	}

	public int getPosition() {
		return position;
	}
}
