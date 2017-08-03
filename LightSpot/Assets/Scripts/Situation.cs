using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Situation : MonoBehaviour {

	GameManager game;
	

	public bool isPlayerSpotted = false;
	public GridNode lastSpottedNode;

	void Start(){
		game = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		
	}

	public void MaxSecurity(){
		game.MaxSecurity ();
	}
}
