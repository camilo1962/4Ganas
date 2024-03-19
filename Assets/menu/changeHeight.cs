using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeHeight : MonoBehaviour {

	public float maxY;
	public float minY;
	private float wait;

	private float lastUpdate;
	private float y;

	// Use this for initialization
	void Start () {
		lastUpdate = -10f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - lastUpdate > wait) {
			y = Random.Range(minY * 1000, maxY * 1000) / 1000f;
			lastUpdate = Time.time;
			wait = Random.Range (1000, 3000) / 1000f;
		} 

		float newYPosition = Mathf.Lerp (transform.position.y, y, Time.deltaTime / wait);

		Vector3 newPosition = new Vector3 (transform.position.x, newYPosition, transform.position.z);
		transform.position = newPosition;
	}
}
