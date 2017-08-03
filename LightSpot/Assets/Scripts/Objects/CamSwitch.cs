using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamSwitch : MonoBehaviour, IInteractables {

	public Cam cam;
	Text notifyText;

	void Start ()
	{
		notifyText = GameObject.Find ("Notify").GetComponent<Text> ();
	}

	public GameObject ThisGameObject ()
	{
		return gameObject;
	}

	public void Interact(PlayerController pl){
		cam.Disable ();
		transform.gameObject.SetActive (false);
		notifyText.gameObject.SetActive (false);
	}

	public void Notify () {
	notifyText.gameObject.SetActive (true);

		notifyText.text = "Press 'E' To Turn Off a Camera";
		notifyText.transform.position = Camera.main.WorldToScreenPoint (transform.position);}
	
}
