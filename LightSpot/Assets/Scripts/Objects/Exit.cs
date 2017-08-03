using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Exit : MonoBehaviour, IInteractables {
	GameManager game;
	Text notifyText;

	void Start(){
		notifyText = GameObject.Find ("Notify").GetComponent<Text> ();
		game = GameObject.FindObjectOfType<GameManager> ();
	}


	public GameObject ThisGameObject(){
		return gameObject;
	}

	public void Interact(PlayerController pl){
		if(pl.intelPicked ==game.levelIntel)
		{
			game.SetGameWin ();
			notifyText.gameObject.SetActive (false);
		}
	}
	public void Notify () {
		if (game.player.intelPicked == game.levelIntel)
			notifyText.text = "Press 'E' To Escape";
		else
			notifyText.text = "Collect all the Intel before Escaping";
				

	notifyText.gameObject.SetActive (true);
		notifyText.transform.position = Camera.main.WorldToScreenPoint (transform.position);}
	

}
