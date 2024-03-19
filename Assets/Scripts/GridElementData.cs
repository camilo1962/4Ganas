using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElementData {

	private GridElement element;

	//0 is no player, 1 is first player, 2 is second player
	public int player;

	public GridElementData(int player) {
		this.player = player;
	}

	public GridElementData(int player, GridElement element) {
		this.player = player;
		this.element = element;
	}

	public void setPlayer(int player) {
		this.player = player;
		if (element != null) {
			element.setPlayer (player);
		}
	}

	public void setPlayerWithoutAnimation(int player) {
		this.player = player;
		if (element != null) {
			element.setPlayerWithoutAnimation (player);
		}
	}
}

