using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createCubes : MonoBehaviour {

	public GameObject cube;
	public int cubeRowsHalf;
	public float gap;

	// Use this for initialization
	void Start () {
		initCubes ();
	}
	
	private void initCubes() {
		for (int row = -cubeRowsHalf; row < cubeRowsHalf; row++) {
			for (int column = -cubeRowsHalf; column < cubeRowsHalf; column++) {			
				Instantiate (cube, new Vector3 (row + gap * row, 0, column + gap * column), Quaternion.identity);
			}
		}
	}
}
