using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {

	//GridPrefab has a size of 1 unit
	public GameObject gridPrefab;
	public GameObject inputPrefab;

	private GameBoardData boardData;

	private GameObject[] input;

	public static bool isPreview;
	private int[ , ] clonedValues;

	private int boardRows;
	private int boardColumns;

	// Use this for initialization
	void Start () {
		boardData = getGameBoardData ();

		input = new GameObject[boardRows * 2 + boardColumns * 2];

		isPreview = false;
		previousPreviewPosition = -1;

		initGameBoard ();
		initGameBoardInput ();
	}

	public void reset() {
		clonedValues = new int[boardRows, boardColumns];
		isPreview = false;
		previousPreviewPosition = -1;

		for (int i = 0; i < boardRows; i++) {
			for (int j = 0; j < boardColumns; j++) {
				clonedValues [i, j] = 0;
			}
		}
	}

	private void initGameBoardInput() {
		float cameraSize = getCameraSize ();
		float scaleSprite = getScaleSprite (cameraSize);

		float boardX = (boardColumns + 2) * scaleSprite;
		float boardY = (boardRows + 2) * scaleSprite;		

		float spriteX, spriteY;

		for (int i = 0; i < 2 * boardRows + 2 * boardColumns; i++) {
			if (i < boardColumns) {
				//TOP
				spriteX = -(boardX / 2) + scaleSprite / 2 + scaleSprite * (i + 1); //i + 1 because we have to start from the 2nd position and not the first
				spriteY = (boardY / 2) - scaleSprite / 2;
			} else if (i < boardColumns + boardRows) {
				//RIGHT
				spriteX = (boardX / 2) - scaleSprite / 2;
				spriteY = (boardY / 2) - scaleSprite / 2 - scaleSprite * (i + 1 - boardColumns);
			} else if (i < boardColumns * 2 + boardRows) {
				//BOTTOM
				spriteX = (boardX / 2) - scaleSprite / 2 - scaleSprite * (i + 1 - boardColumns - boardRows);
				spriteY = -(boardY / 2) + scaleSprite / 2;
			} else {
				//LEFT
				spriteX = -(boardX / 2) + scaleSprite / 2;
				spriteY = -(boardY / 2) + scaleSprite / 2	 + scaleSprite * (i + 1 - boardColumns * 2 - boardRows);
			}

			GameObject inputObject = Instantiate (inputPrefab, new Vector3 (spriteX, spriteY, 0), Quaternion.identity);
			inputObject.transform.localScale = new Vector3 (scaleSprite, scaleSprite, scaleSprite);

			InputElement inputElement = inputObject.GetComponent<InputElement> ();
			inputElement.setPosition (i);

			input [i] = inputObject;
		}
	}

	private void initGameBoard() {
		float cameraSize = getCameraSize ();

		float scaleSprite = getScaleSprite (cameraSize);

		float boardX = boardColumns * scaleSprite;
		float boardY = boardRows * scaleSprite;

		float spriteX, spriteY;

		for (int row = 0; row < boardRows; row++) {
			for (int column = 0; column < boardColumns; column++) {
				spriteX = -(boardX / 2) + scaleSprite / 2 + column * scaleSprite;
				spriteY = (boardY / 2) - scaleSprite / 2 - row * scaleSprite;

				GameObject gridObject = Instantiate(gridPrefab, new Vector3(spriteX, spriteY, 0), Quaternion.identity);
				gridObject.transform.localScale = new Vector3 (scaleSprite, scaleSprite, scaleSprite);

				boardData.initGameBoard (row, column, gridObject.GetComponent<GridElement> ());
			}
		}
	}

	private float getScaleSprite(float gameBoardView) {
		if (boardColumns >= boardRows) {
			return (gameBoardView / (boardColumns + 2));
		} else {
			return (gameBoardView / (boardRows + 2));
		}
	}

	/**
	 * @return the size of the camera in unity units (the lower value from width and height)
	*/
	private float getCameraSize() {
		float screenAspect = (float) Screen.width / (float) Screen.height;
		float camHalfHeight = Camera.main.orthographicSize;
		float camHalfWidth = screenAspect * camHalfHeight;
		if (camHalfHeight >= camHalfWidth) {
			return camHalfWidth * 2.0f;
		} else {
			return camHalfHeight * 2.0f - 2;
		}
	}

	public void setInputElementClicked(int position) {
		foreach (GameObject inputObject in input) {
			InputElement inputElement = inputObject.GetComponent<InputElement> ();
			if (inputElement.getPosition () == position) {
				inputElement.inserted ();
			} else {
				inputElement.resetColor ();
			}
		}
	}

	int previousPreviewPosition;

	public void showPreview(int position, int player) {
		if (player == GameManager.FIRSTPLAYER) {
			player = GameManager.FIRSTPLAYERPEV;
		} else if(player == GameManager.SECONDPLAYER){
			player = GameManager.SECONDPLAYERPREV;
		}

		if (isPreview && previousPreviewPosition == position) {
			//Preview did not change -> do nothing
		} else if(isPreview && previousPreviewPosition != position) {
			//Preview positon changed, reset field to original and insert preview
			loadElements (clonedValues);

			boardData.insert (position, player);
			previousPreviewPosition = position;
		} else {
			previousPreviewPosition = position;
			clonedValues = saveElements ();

			boardData.insert (position, player);

			isPreview = true;
		}
	}

	public void cancelPreview() {
		if (isPreview) {
			isPreview = false;
			if (clonedValues != null)
				loadElements (clonedValues);
		}
	}

	private int[,] saveElements() {
		return boardData.saveElements ();
	}

	private void loadElements(int[,] saved) {
		boardData.loadElements (saved);
	}

	public GameBoardData getGameBoardData() {
		//TODO user have to define these values
		boardRows = 7;
		boardColumns = 7;

		if (boardData == null) {
			boardData = new GameBoardData (boardRows, boardColumns);
		}

		return boardData;
	}
}