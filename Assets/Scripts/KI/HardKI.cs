using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HardKI : Player, AiListener {

	private GameBoardData board;

	private int playerMe;
	private int deep;

	private int countRatings;
	private List<int> validTurns;

	public HardKI (int playerMe, int deep) {
		this.playerMe = playerMe;
		countRatings = 0;
		this.deep = deep;
	}

	public void calcNextMove (int player, GameBoardData gameBoard) {
		countRatings = 0;

		board = gameBoard;
		validTurns = board.getValidTurns ();

		turnHighestRating = validTurns [0];
		highestRating = int.MinValue;

		for (int i = 0; i < validTurns.Count; i++) {
			DeepSearch deepSearch = new DeepSearch (board, validTurns[i], deep, playerMe, playerMe, int.MinValue, int.MaxValue);
			deepSearch.setAiListener (this);
			deepSearch.Start ();
		}
	}

	//Choose the highest rating
	int highestRating;
	int turnHighestRating;

	public void calculatedRating(int turn, int rating) {
		countRatings++;

		//Debug.Log ("Rating: " + rating + ", validTurn: " + turn);
		if (rating > highestRating) {
			highestRating = rating;
			turnHighestRating = turn;
		}
	}

	public bool finishedCalc() {
		if (validTurns == null) {
			return false;
		}
		return countRatings == validTurns.Count;
	}

	public int getMove() {
		return turnHighestRating;
	}

	public String getName() {
		return "Hard AI";
	}
}

