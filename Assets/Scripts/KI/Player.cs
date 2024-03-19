using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Player {

	void calcNextMove (int player, GameBoardData gameBoard);

	bool finishedCalc ();
	int getMove ();

	string getName();
}
