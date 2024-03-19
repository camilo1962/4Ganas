using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardInput : MonoBehaviour {

	//is set in the inspector thasts why its public
	public GameManager gameManager;

	public GameBoard gameBoard; //only to show preview

	private bool humansTurn;

	private bool hoverBefore;

	void Start() {
		hoverBefore = false;
		humansTurn = true;
	}

	// Update is called once per frame
	void Update () {
		if (!GameManager.isGameOver() && humansTurn) {
			checkInputTouch ();
			checkMouseInput ();
		}

		if (!hoverBefore || !humansTurn) {
			gameBoard.cancelPreview ();
		}
	}

	private void checkInputTouch() {
		if (Input.touchCount == 1) {
			RaycastHit2D hitInfo = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position), Vector2.zero);
			// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
			if (hitInfo) {
				hover (hitInfo.transform.gameObject);
			} else {
				hoverBefore = false;
			}
		}
	}

	private void checkMouseInput() {
		if (Input.GetMouseButtonUp (0)) {
			Vector2 pos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			RaycastHit2D hitInfo = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (pos), Vector2.zero);
			// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
			if (hitInfo) {
				click (hitInfo.transform.gameObject);
			}
		} else if (!Input.GetMouseButton (0) && Input.mousePresent) {
			Vector2 pos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			RaycastHit2D hitInfo = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (pos), Vector2.zero);
			// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
			if (hitInfo) {
				hover (hitInfo.transform.gameObject);
			} else {
				hoverBefore = false;
			}
		}
	}

	private void hover(GameObject gameObject) {
		InputElement input = gameObject.GetComponent<InputElement> ();
		if (input != null && gameBoard.getGameBoardData().canInsert(input.getPosition())) {
			gameBoard.showPreview (input.getPosition(), GameManager.getActualPlayer ());
			input.onHover ();

			hoverBefore = true;
		} 
	}

	private void click(GameObject gameObject) {
		InputElement input = gameObject.GetComponent<InputElement> ();
		if (input != null && gameBoard.getGameBoardData().canInsert (input.getPosition())) {
			gameBoard.cancelPreview ();

			gameManager.click (input.getPosition ());
		}
	}

	public void setHumansTurn(bool humansTurn) {
		this.humansTurn = humansTurn;
	}
}
