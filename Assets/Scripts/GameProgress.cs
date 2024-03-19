using System;
using System.Collections;
using System.Collections.Generic;

public class GameProgress {

	public List<int[,]> boards;
	public List<int> insertPos;

	private int curPos;
	
	public GameProgress () {
		boards = new List<int[,]> ();
		insertPos = new List<int> ();
	}

	public void newState(int[,] board, int insertPos) {
		boards.Add (board);
		this.insertPos.Add (insertPos);
		curPos = boards.Count - 1;
	}

	public int[,] getPreviousBoard() {
		curPos--;
		if (curPos < 0)
			curPos = 0;

		return boards[curPos];
	}

	public int getInsertPos() {
		return insertPos [curPos];
	}

	public int[,] getNextBoard() {
		curPos++;
		if (curPos == boards.Count)
			curPos = boards.Count - 1;

		return boards [curPos];
	}
}