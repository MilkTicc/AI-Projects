using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intel : MonoBehaviour, IInteractables {

	Text notifyText;

	void Start(){
		notifyText = GameObject.Find ("Notify").GetComponent<Text> ();
	}

	public GameObject ThisGameObject(){
		return gameObject;
	}

	public void Interact(PlayerController pl){
		gameObject.SetActive (false);
		pl.intelPicked += 1;
		notifyText.gameObject.SetActive (false);
		
	}

	public void Notify(){
		notifyText.gameObject.SetActive (true);
		
		notifyText.text = "Press 'E' To Pick Up this Intel";
		notifyText.transform.position = Camera.main.WorldToScreenPoint(transform.position);
	}

}
