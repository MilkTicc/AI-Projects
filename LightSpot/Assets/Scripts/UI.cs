using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
	GameManager game;
	public Text quest1;
	public Text quest2;
	public Image tick1;
	public Image tick2;
	public Image security;
	public GameObject questPanel;
	public GameObject securityPanel;
	// Use this for initialization
	void Start () {
		game = FindObjectOfType<GameManager> ();
		//quest2.text = "Escape with ALL the Intel";
		//quest2.color = Color.gray;
		//quest2.transform.position
		quest1.text = "Collect Intels " + game.player.intelPicked + " / " + game.levelIntel;
		quest2.color = new Color(0.7f,0.7f,0.7f);
		securityPanel.transform.position = new Vector2 (Screen.width/2, Screen.height*0.93f);
		questPanel.transform.position = new Vector2 (Screen.width * 0.97f , Screen.height * 0.93f);
		securityPanel.transform.localScale = Vector3.one * Camera.main.pixelWidth / 1920;
		questPanel.transform.localScale = Vector3.one * Camera.main.pixelWidth / 1920;
		//security.rectTransform.w
	}


	// Update is called once per frame
	void Update () {
		//if (game.player.intelPicked < game.levelIntel)
			quest1.text = "Collect Intels " + game.player.intelPicked + " / " + game.levelIntel;

		security.rectTransform.localScale = new Vector3 (game.scurityLevel / 60, 1, 1);
		security.color = new Color (game.scurityLevel / 60, 1 - game.scurityLevel / 60, 0)*1.5f;

		if (game.player.intelPicked == game.levelIntel){
			quest1.color= new Color (0.7f, 0.7f, 0.7f);
			tick1.gameObject.SetActive (true);
			quest2.color = Color.white;
		}
		if(game.gameWin){
			tick2.gameObject.SetActive (true);
		}
		//else
		//	quest1.text = "Find the Exit and Escape";
	}
}
