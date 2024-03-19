using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyKI : Player {

	int turn = -1;

	public void calcNextMove(int player, GameBoardData gameBoard) {
		//Get valid turns and then choose one randomly
		List<int> validTurns = gameBoard.getValidTurns();

		turn = Random.Range (0, validTurns.Count - 1);
	}

	public bool finishedCalc() {
		return turn != -1;
	}

	public int getMove() {
		return turn;
	}

	public string getName() {
		return "Easy AI";
	}
}
