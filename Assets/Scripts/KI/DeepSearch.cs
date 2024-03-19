using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DeepSearch : BaseThread {

	private int deep;
	private int insertPosition;
	private GameBoardData board;

	private int playerMe;
	private int playersTurn;

	private int[] weights;

	private AiListener listener;
	private int rating;

	private int lowerBound, upperBound;

	public override void RunThread() {
		rating = getRating ();
		if (listener != null) {
			listener.calculatedRating (insertPosition, rating);
		}
	}

	public void setAiListener(AiListener listener) {
		this.listener = listener;
	}

	/**
	 * Simulates all moves with the given deep and first insert position 
	 * and calculates a rating for this combination
	 * @firstInsertPos has to be a valid insert position for the given GameBoard
	 * @deep 1 is only the firstInsertPos move, 0 is no move
	 * @playersTurn which player makes the next move
	 * @playerMe which player is me. PlayerOne or PlayerTwo
	 */
	public DeepSearch (GameBoardData board, int firstInsertPos, int deep, int playersTurn, int playerMe, int lowerBound, int upperBound) {
		this.deep = deep;
		insertPosition = firstInsertPos;
		this.board = board.clone();
		this.playerMe = playerMe;
		this.playersTurn = playersTurn;
		this.lowerBound = lowerBound;
		this.upperBound = upperBound;

		int singleWeight = 10, doubleWeight = 100, tripleWeight = 10000;
		weights = new int[7];
		weights [0] = 0;
		weights [1] = singleWeight;
		weights [2] = doubleWeight;
		weights [3] = tripleWeight;
		weights [4] = tripleWeight * 10;
		weights [5] = tripleWeight * 100;
		weights [6] = tripleWeight * 1000;
	}

	private int getRating() {
		board.insert(insertPosition, playersTurn);
		deep--;

		//Fist check for winner or draw
		int winner = board.calculateWinner(board);
		if (winner == playerMe) {
			//Debug.Log ("Me winner");
			return int.MaxValue;
		} else if (winner == GameManager.DRAW) {
			//Debug.Log ("Draw");
			return 0;
		} else if (winner != GameManager.NONE) {
			//enemy wins
			//Debug.Log("enemy winner");
			return int.MinValue;
		}

		if (deep > 0) {
			int newPlayersTurn = playersTurn;

			if (playersTurn == GameManager.FIRSTPLAYER) {
				newPlayersTurn = GameManager.SECONDPLAYER;
			} else if (playersTurn == GameManager.SECONDPLAYER) {
				newPlayersTurn = GameManager.FIRSTPLAYER;
			}

			int choosenRating = 0;
			List<int> turns = board.getValidTurns ();

			for (int i = 0; i < turns.Count; i++) {
				DeepSearch search;

				if (i == 0) {
					search = new DeepSearch(board.clone(), turns[i], deep, newPlayersTurn, playerMe, lowerBound, upperBound);
				}

				if (newPlayersTurn == playerMe) { 
					search = new DeepSearch (board.clone (), turns [i], deep, newPlayersTurn, playerMe, choosenRating, upperBound);
				} else {
					search = new DeepSearch (board.clone (), turns [i], deep, newPlayersTurn, playerMe, lowerBound, choosenRating);
				}

				int rating = search.getRating ();

				if (i == 0) {
					choosenRating = rating;
				} else {
					//if this is my move than choose the highest value
					//if this is not my move than choose the lowest value
					if (newPlayersTurn == playerMe) {
						if (rating > choosenRating) {
							choosenRating = rating;
							if (choosenRating == int.MaxValue) {
								//we found one of the best moves now we can exit
								return choosenRating;
							} else if(choosenRating <= lowerBound) {
								return lowerBound;
							}
						}
					} else {
						if (rating < choosenRating) {
							choosenRating = rating;
							if (choosenRating == int.MinValue) {
								//we found one of the best moves now we can exit
								return choosenRating;
							} else if (choosenRating >= upperBound) {
								return upperBound;
							}
						}
					}
				}
			}

			return choosenRating;
		} else {
			return calculateRating ();
		}
	}

	private int calculateRating() {
		int rating = 0;
		int playerBefore = GameManager.NONE; //the last player is saved here. Will never be GameManager.NONE excpet at the beginning
		int sameStonesCount = 0;
		int currentPlayer;

		//count the same stones in a row
		for (int row = 0; row < board.boardRows; row++) {
			for (int column = 0; column < board.boardColumns; column++) {
				currentPlayer = board.getPlayerAt (row, column);

				if (currentPlayer == GameManager.NONE) {
					//do nothing. Rows wiht a gap between the stones are counted like rows without a gap
				} else if (playerBefore == GameManager.NONE) {
					//first stone in the row
					playerBefore = currentPlayer;
					sameStonesCount = 1;
				} else if (playerBefore == currentPlayer) {
					//same stone as before 
					sameStonesCount++;
				} else {
					//stone changed -> rate
					rating += rate(sameStonesCount, playerBefore);
					playerBefore = currentPlayer;
					sameStonesCount = 1;
				}

				//if its the last column count the stones to the rating
				if (column == board.boardColumns - 1) {
					rating += rate (sameStonesCount, currentPlayer);
				}
			}
		}

		playerBefore = GameManager.NONE; //the last player is saved here. Will never be GameManager.NONE excpet at the beginning
		sameStonesCount = 0;

		//count the same stones in a column
		for (int column = 0; column < board.boardColumns; column++) {
			for (int row = 0; row < board.boardRows; row++) {
				currentPlayer = board.getPlayerAt (row, column);

				if (currentPlayer == GameManager.NONE) {
					//do nothing. Columns with a gap between the stones are counted like columns without a gap
				} else if (playerBefore == GameManager.NONE) {
					//first stone in the column
					playerBefore = currentPlayer;
					sameStonesCount = 1;
				} else if (playerBefore == currentPlayer) {
					//same stone as before 
					sameStonesCount++;
				} else {
					//stone changed -> rate
					rating += rate(sameStonesCount, playerBefore);
					playerBefore = currentPlayer;
					sameStonesCount = 1;
				}

				//if its the last row count the stones to the rating
				if (row == board.boardRows - 1) {
					rating += rate (sameStonesCount, currentPlayer);
				}
			}
		}

		//count the same stones diagonal
		/*
		for (int row = 0; row < board.boardRows; row++) {
			for (int column = 0; column < board.boardColumns; column++) {
				currentPlayer = board.getPlayerAt (row, column);

				if (currentPlayer == GameManager.NONE) {
					//do nothing. Rows wiht a gap between the stones are counted like rows without a gap
				} else if (playerBefore == GameManager.NONE) {
					//first stone in the row
					playerBefore = currentPlayer;
					sameStonesCount = 1;
				} else if (playerBefore == currentPlayer) {
					//same stone as before 
					sameStonesCount++;
				} else {
					//stone changed -> rate
					rating += rate(sameStonesCount, playerBefore);
					playerBefore = currentPlayer;
					sameStonesCount = 1;
				}

				//if its the last column count the stones to the rating
				if (column == board.boardColumns - 1) {
					rating += rate (sameStonesCount, currentPlayer);
				}
			}
		}*/


		return rating;
	}

	private int rate(int sameStonesCount, int player) {
		if (sameStonesCount >= weights.Length) {
			sameStonesCount = weights.Length - 1;
		}

		if (player == playerMe) {
			//my points
			return weights [sameStonesCount];
		} else if (player == GameManager.NONE) {
			//no points
			return 0;
		} else {
			//points for enemy
			return -weights[sameStonesCount];
		}
	}

}

