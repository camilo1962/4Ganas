using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardData {

	public int boardRows;
	public int boardColumns;

	private GridElementData[,] board;

	public GameBoardData(int rows, int columns) {
		boardRows = rows;
		boardColumns = columns;

		board = new GridElementData[boardRows, boardColumns];
	}

	public GameBoardData (int[,] board) {
		boardRows = board.GetLength (0);
		boardColumns = board.GetLength (1);

		this.board = new GridElementData[boardRows, boardColumns];

		for (int row = 0; row < boardRows; row++) {
			for (int column = 0; column < boardColumns; column++) {
				this.board [row, column] = new GridElementData (board[row,column]); 
			}
		}
	}

	public void initGameBoard(int row, int column, GridElement gridElement) {
		board[ row, column] = new GridElementData (GameManager.NONE, gridElement);
	}

	public void reset() {
		for (int i = 0; i < boardRows; i++) {
			for (int j = 0; j < boardColumns; j++) {
				board[ i, j].setPlayer (GameManager.NONE);
			}
		}
	}

	public void insert(int position, int player) {
		if(position < boardColumns) { //on top
			insertIntoColumn(position, true, player);
		} else if(position < boardColumns + boardRows) { //right side
			insertIntoRow(position - boardColumns, false, player);
		} else if(position < boardColumns * 2 + boardRows) { //bottom
			insertIntoColumn(boardColumns * 2 + boardRows - position - 1, false, player);
		} else if(position < boardColumns * 2 + boardRows * 2) { //left side
			insertIntoRow(boardColumns * 2 + boardRows * 2 - position - 1, true, player);
		}
	}

	private void insertIntoRow(int row, bool toRight, int value) {
		int lastFreeRow;

		if(toRight) {
			lastFreeRow = boardRows - 1;
			for(int i = lastFreeRow; i >= 0; i--) {
				if( board[row, i].player == 0 ) {
					//Search for next non 0 field
					for(int j = i; j>= 0; j--) {
						if(board[row, j].player != 0) {
							board[row, i].setPlayer(board[row, j].player);
							board[ row, j].setPlayer (0);
							break;
						}
					}
				}
			}

			while(board[row, lastFreeRow].player != 0) {
				lastFreeRow--;
			}
		} else {
			lastFreeRow = 0;
			for(int i = 0; i < boardRows; i++) {
				if( board[row, i].player == 0 ) {
					//Search for next non 0 field
					for(int j = i; j < boardRows; j++) {
						if(board[row, j].player != 0) {
							board[row, i].setPlayer(board[row, j].player);
							board[ row, j].setPlayer (0);
							break;
						}
					}
				}
			}

			while(board[row, lastFreeRow].player != 0) {
				lastFreeRow++;

				if(lastFreeRow > 6) {
					Debug.Log("Error");
				}
			}
		}

		if(board[row, lastFreeRow].player == 0) {
			board[row, lastFreeRow].setPlayer(value);
		}
	}

	private void insertIntoColumn(int column, bool toBottom, int value) {
		int lastFreeColumn;

		if(toBottom) {
			lastFreeColumn = boardColumns - 1;
			for(int i = boardColumns - 1; i >= 0; i--) {
				if( board[i, column].player == 0 ) {
					//Search for next non 0 field
					for(int j = i; j>= 0; j--) {
						if(board[j, column].player != 0) {
							board[i, column].setPlayer(board[j, column].player);
							board[j, column].setPlayer(0);
							break;
						}
					}
				}
			}

			while(board[lastFreeColumn, column].player != 0) {
				lastFreeColumn--;
			}
		} else {
			lastFreeColumn = 0;
			for(int i = 0; i < boardColumns; i++) {
				if( board[i, column].player == 0 ) {
					//Search for next non 0 field
					for(int j = i; j < boardColumns; j++) {
						if(board[j, column].player != 0) {
							board[i, column].setPlayer(board[j, column].player);
							board[j, column].setPlayer(0);
							break;
						}
					}
				}
			}
			while(board[lastFreeColumn, column].player != 0) {
				lastFreeColumn++;
			}
		}

		if(board[lastFreeColumn, column].player == 0) {
			board[lastFreeColumn, column].setPlayer(value);
		}
	}

	public bool canInsert(int position) {
		if(position < boardColumns) { //on top
			return !fullColumn(position);
		} else if(position < boardColumns + boardRows) { //right side
			return !fullRow(position - boardColumns);
		} else if(position < boardColumns * 2 + boardRows) { //bottom
			return !fullColumn(boardColumns * 2 + boardRows - position - 1);
		} else if(position < boardColumns * 2 + boardRows * 2) { //left side
			return !fullRow(boardColumns * 2 + boardRows * 2 - position - 1);
		}

		return false;
	}

	private bool fullColumn(int column) {
		for(int i = 0; i < boardRows; i++) {
			if(board[i, column].player != GameManager.FIRSTPLAYER && board[i, column].player != GameManager.SECONDPLAYER) {
				return false;
			}
		}

		return true;
	}

	private bool fullRow(int row) {
		for(int i = 0; i < boardColumns; i++) {
			if(board[row, i].player != GameManager.FIRSTPLAYER && board[row, i].player != GameManager.SECONDPLAYER) {
				return false;
			}
		}

		return true;
	}

	public int calculateWinner(GameBoardData gameBoard) {
		GridElementData[ , ] board = gameBoard.getGameBoard ();
		int rows = gameBoard.boardRows;
		int columns = gameBoard.boardColumns;

		int column,row, prev, count, playerAtField, rowCounter;
		bool firstPlayerWon = false;
		bool secondPlayerWon = false;

		//check rows
		for(column = 0; column < columns; column++) {
			count = 1;
			prev = 0;
			for(row = 0; row < rows; row++) {
				playerAtField = board[row, column].player;
				if(playerAtField != prev) {
					prev = playerAtField;
					count = 1;
				} else if(playerAtField == GameManager.FIRSTPLAYER) { //first player
					count++;
					if(count >= GameManager.tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == GameManager.SECONDPLAYER) { //second player
					count++;
					if(count >= GameManager.tokensToWin) {
						secondPlayerWon = true;
					}
				}
			}
		}

		//check columns
		for(row = 0; row < rows; row++) {
			count = 1;
			prev = 0;
			for(column = 0; column < columns; column++) {
				playerAtField = board[row, column].player;
				if(playerAtField != prev) {
					prev = playerAtField;
					count = 1;
				} else if(playerAtField == GameManager.FIRSTPLAYER) { //first player
					count++;
					if(count >= GameManager.tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == GameManager.SECONDPLAYER) { //second player
					count++;
					if(count >= GameManager.tokensToWin) {
						secondPlayerWon = true;
					}
				}
			}
		}

		//check diagonal
		for(column = 0; column < columns; column++) {
			count = 1;
			prev = GameManager.NONE;
			rowCounter = 0;
			while(column + rowCounter < columns && rowCounter < rows) {
				playerAtField = board[rowCounter, column + rowCounter].player;
				if(playerAtField != prev) {
					prev = playerAtField;
					count = 1;
				} else if(playerAtField == GameManager.FIRSTPLAYER) { //first player
					count++;
					if(count >= GameManager.tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == GameManager.SECONDPLAYER) { //second player
					count++;
					if(count >= GameManager.tokensToWin) {
						secondPlayerWon = true;
					}
				}

				rowCounter++;
			}
		}
		for(row = 0; row < rows; row++) {
			count = 1;
			prev = 0;
			rowCounter = 0;
			while(row + rowCounter < rows && rowCounter < columns) {
				playerAtField = board[row + rowCounter, rowCounter].player;
				if(playerAtField != prev) {
					prev = playerAtField;
					count = 1;
				} else if(playerAtField == GameManager.FIRSTPLAYER) { //first player
					count++;
					if(count >= GameManager.tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == GameManager.SECONDPLAYER) { //second player
					count++;
					if(count >= GameManager.tokensToWin) {
						secondPlayerWon = true;
					}
				}

				rowCounter++;
			}
		}

		for(column = 0; column < columns; column++) {
			count = 1;
			prev = 0;
			rowCounter = 0;
			while(column - rowCounter >= 0 && rowCounter < rows) {
				playerAtField = board[rowCounter, column - rowCounter].player;
				if(playerAtField != prev) {
					prev = playerAtField;
					count = 1;
				} else if(playerAtField == GameManager.FIRSTPLAYER) { //first player
					count++;
					if(count >= GameManager.tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == GameManager.SECONDPLAYER) { //second player
					count++;
					if(count >= GameManager.tokensToWin) {
						secondPlayerWon = true;
					}
				}

				rowCounter++;
			}
		}
		for(row = 0; row < rows; row++) {
			count = 1;
			prev = 0;
			rowCounter = 0;
			while(row - rowCounter >= 0 && rowCounter < columns) {
				playerAtField = board[row - rowCounter, rowCounter].player;
				if(playerAtField != prev) {
					prev = playerAtField;
					count = 1;
				} else if(playerAtField == GameManager.FIRSTPLAYER) { //first player
					count++;
					if(count >= GameManager.tokensToWin) {
						firstPlayerWon = true;
					}
				} else if(playerAtField == GameManager.SECONDPLAYER) { //second player
					count++;
					if(count >= GameManager.tokensToWin) {
						secondPlayerWon = true;
					}
				}

				rowCounter++;
			}
		}

		if (!movePossible() || (firstPlayerWon && secondPlayerWon)) {
			return GameManager.DRAW;
		} else if (firstPlayerWon) {
			return GameManager.FIRSTPLAYER;
		} else if (secondPlayerWon) {
			return GameManager.SECONDPLAYER;
		} else {
			return GameManager.NONE;
		}
	}

	public int getPlayerAt(int row, int column) {
		return board [row, column].player;
	}

	private bool movePossible() {
		for (int row = 0; row < 2 * (boardRows + boardColumns); row++) {
			if (canInsert (row)) {
				return true;
			}
		}

		return false;
	}

	public GridElementData[,] getGameBoard() {
		return board;
	}

	public int[,] saveElements() {
		int[,] clone = new int[ boardRows, boardColumns];
		for (int i = 0; i < boardRows; i++) {
			for (int j = 0; j < boardColumns; j++) {
				clone [i, j] = board [i, j].player;
			}
		}

		return clone;
	}

	public void loadElements(int[,] saved) {
		for (int i = 0; i < boardRows; i++) {
			for (int j = 0; j < boardColumns; j++) {
				board [i, j].setPlayerWithoutAnimation(saved[i, j]);
			}
		}
	}

	public List<int> getValidTurns() {
		List<int> validTurns = new List<int>();
		for (int i = 0; i < boardColumns * 2 + boardRows * 2; i++) {
			if (canInsert (i)) {
				validTurns.Add (i);
			} 
		}

		return validTurns;
	}

	public int[,] getBoardAsArray() {
		return saveElements ();
	}

	public GameBoardData clone() {
		return new GameBoardData (getBoardAsArray());
	}

}

