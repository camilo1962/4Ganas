using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public sounds sounds;
	public GameBoard gameBoardObject;
	private GameBoardData gameBoardData;
	public GameBoardInput gameBoardInput;
	public GameObject pauseCanvas;
	public GameObject gameCanvas;
	public GameObject aiCanvas;
	public GameObject navigateBoardCanvas;
	public GameObject buttonRestart;
	public GameObject buttonResume;
	public GameObject buttonShowBoard;
	public Background background;

	private int kiPlayer; //which player (first or second) is the ki, 0 is no ki
	private Player kiImplementation; //difficulty
	private bool calculatingTurn;

	private static int actualPlayer;
	private static bool gameOver;
	public static int tokensToWin = 4;

	public static int NONE = 0;
	public static int FIRSTPLAYER = 1;
	public static int SECONDPLAYER = 2;
	public static int DRAW = 3;
	public static int FIRSTPLAYERPEV = 4;
	public static int SECONDPLAYERPREV = 5;

	private Text winnerText;

	private string firstPlayerName;
	private string secondPlayerName;

	private GameProgress gameProgress;
	private Tutorial tutorial;

	// Use this for initialization
	void Start () {
		tutorial = GetComponent<Tutorial> ();
		gameBoardData = gameBoardObject.getGameBoardData ();

		winnerText = pauseCanvas.GetComponentInChildren<Text> ();

		if (MenuAttributes.vsKi) {
			if (MenuAttributes.difficulty == 1) {
				kiPlayer = SECONDPLAYER;
				kiImplementation = new HardKI (SECONDPLAYER, 2);
				firstPlayerName = MenuAttributes.firstPlayerName;
				secondPlayerName = kiImplementation.getName ();
			} else if (MenuAttributes.difficulty == 2) {
				kiPlayer = SECONDPLAYER;
				kiImplementation = new HardKI (SECONDPLAYER, 3);
				firstPlayerName = MenuAttributes.firstPlayerName;
				secondPlayerName = kiImplementation.getName ();
			} else {
				kiPlayer = FIRSTPLAYER;
				kiImplementation = new HardKI (FIRSTPLAYER, 3);
				firstPlayerName = kiImplementation.getName ();
				secondPlayerName = MenuAttributes.firstPlayerName;
			}
		} else {
			kiPlayer = NONE;
			firstPlayerName = MenuAttributes.firstPlayerName;
			secondPlayerName = MenuAttributes.secondPlayerName;
		}

		restart ();
	}

	void Update() {
		if (actualPlayer == kiPlayer && !gameOver) {
			//calculate ki turn and send it to gameBoard and do this asynchron
			if (!calculatingTurn) {
				Debug.Log ("Start");
				calculatingTurn = true;

				kiImplementation.calcNextMove (actualPlayer, gameBoardData);
			} else if (kiImplementation.finishedCalc ()) {
				Debug.Log ("Final");
				aiCanvas.SetActive (false);
				//Finsiehd calculating
				insert(kiImplementation.getMove());
			} else {
				aiCanvas.SetActive (true);
			}
		}
	}

	public void restart() {
		gameProgress = new GameProgress ();
		tutorial.defaultState ();

		sounds.playTick ();

		setActualPlayer (FIRSTPLAYER);
		pauseCanvas.SetActive (false);
		gameCanvas.SetActive (true);
		aiCanvas.SetActive (false);
		navigateBoardCanvas.SetActive (false);
		gameOver = false;
		calculatingTurn = false;

		gameBoardObject.reset ();
		gameBoardData.reset ();
	}

	public void backToMenu() {
		sounds.playTick ();

		SceneManager.LoadScene ("menu");
	}

	public void pause() {
		sounds.playTick ();

		if (!gameOver) { //game was paused
			winnerText.text = "";
			buttonShowBoard.SetActive (false);
			pauseCanvas.SetActive (true);
			gameCanvas.SetActive (false);

			buttonResume.SetActive (true);
			buttonRestart.SetActive (false);
			aiCanvas.SetActive (false);
			gameOver = true;
		} else { //game was ended and paused
			pauseCanvas.SetActive (true);
			gameCanvas.SetActive (false);
			aiCanvas.SetActive (false);

			buttonResume.SetActive (false);
			buttonRestart.SetActive (true);
			gameOver = true;
		}
	}

	public void resume() {
		sounds.playTick ();

		pauseCanvas.SetActive (false);
		gameCanvas.SetActive (true);
		aiCanvas.SetActive (false);

		buttonResume.SetActive (false);
		buttonRestart.SetActive (true);
		gameOver = false;
	}

	public void click(int pos) {
		insert (pos);
	}

	public void loadPreviousState() {
		gameBoardData.loadElements (gameProgress.getPreviousBoard ());
		gameBoardObject.setInputElementClicked (gameProgress.getInsertPos ());
	}

	public void loadNextState() {
		gameBoardData.loadElements (gameProgress.getNextBoard ());
		gameBoardObject.setInputElementClicked (gameProgress.getInsertPos ());
	}

	private void insert(int pos) {
		gameBoardObject.setInputElementClicked (pos);
		gameBoardData.insert (pos, actualPlayer);

		gameProgress.newState (gameBoardData.getBoardAsArray (), pos);

		gameBoardChanged ();
		changePlayer ();
	}

	public void showBoard() {
		sounds.playTick ();

		pauseCanvas.SetActive (false);
		gameCanvas.SetActive (true);
	}

	private void gameEnded(int winner) {
		buttonShowBoard.SetActive (true);
		pauseCanvas.SetActive (true);
		gameCanvas.SetActive (false);
		aiCanvas.SetActive (false);
		navigateBoardCanvas.SetActive (true);

		tutorial.disable ();

		gameOver = true;

		if (MenuAttributes.vsKi && kiPlayer == winner) {
			sounds.playLooser ();
		} else {
			sounds.playWinner ();
		}

		if (winner == FIRSTPLAYER) {
			winnerText.text = LocalizationText.GetText ("El Ganador es") + firstPlayerName;
			sounds.playWinner ();
		} else if (winner == SECONDPLAYER) {
			winnerText.text = LocalizationText.GetText ("El Ganador es") + secondPlayerName;
		} else if (winner == DRAW) {
			winnerText.text = LocalizationText.GetText ("draw");
			sounds.playLooser ();
		}
	}

	private void gameBoardChanged() {
		sounds.playTick ();

		int winner = gameBoardData.calculateWinner(gameBoardData);

		if (winner != NONE) {
			gameEnded (winner);
		}
	}

	private void changePlayer() {
		if (actualPlayer == FIRSTPLAYER)
			setActualPlayer (SECONDPLAYER);
		else 
			setActualPlayer(FIRSTPLAYER);
	}

	private void setActualPlayer(int player) {
		actualPlayer = player;

		if (player == FIRSTPLAYER) {
			background.setBackground (true);
		} else if (player == SECONDPLAYER) {
			background.setBackground (false);
		}

		if (player == kiPlayer) {
			gameBoardInput.setHumansTurn (false);
		} else {
			gameBoardInput.setHumansTurn (true);
			calculatingTurn = false;
		}
	}

	public static int getActualPlayer() {
		return actualPlayer;
	}

	public static bool isGameOver() {
		return gameOver;
	}
}
