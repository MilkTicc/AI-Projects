using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace AISandbox
{
	public class UI : MonoBehaviour
	{
		public Text brief;
		public Image panel;
		public Text detail;
		public Text detail2;
		public Text hint;
		public TankGame game;

		// Use this for initialization
		void Start ()
		{
			
			brief.transform.position = new Vector2 (Screen.width * 0.83f, Screen.height * 0.97f);
			detail.transform.position = new Vector2 (Screen.width * 0.03f, Screen.height * 0.83f);
			detail2.transform.position = new Vector2 (Screen.width * 0.17f, Screen.height * 0.83f);
			hint.transform.position = new Vector2 (Screen.width * 0.03f, Screen.height * 0.97f);
			hint.text = "Hold <color=yellow>TAB</color> to Show Detail Score of Each Tanks"+"\nCross Over Rate: 70%"+"\nMutation Rate: 5%";
			
		}

		// Update is called once per frame
		void Update ()
		{
			detail.text = "\n" + game.allTanks [0].name +
						  "\n" + game.allTanks [1].name +
						  "\n" + game.allTanks [2].name +
						  "\n" + game.allTanks [3].name +
						  "\n" + game.allTanks [4].name +
						  "\n" + game.allTanks [5].name +
						  "\n" + game.allTanks [6].name +
						  "\n" + game.allTanks [7].name +
						  "\n" + game.allTanks [8].name +
						  "\n" + game.allTanks [9].name +
						  "\n" + game.allTanks [10].name +
						  "\n" + game.allTanks [11].name +
						  "\n" + game.allTanks [12].name +
						  "\n" + game.allTanks [13].name +
						  "\n" + game.allTanks [14].name +
						  "\n" + game.allTanks [15].name +
						  "\n" + game.allTanks [16].name +
						  "\n" + game.allTanks [17].name +
						  "\n" + game.allTanks [18].name +
						  "\n" + game.allTanks [19].name;

			detail2.text = "\n   Current Score: " + game.allTanks [0].InputList.Score +
						   "\n   Current Score: " + game.allTanks [1].InputList.Score +
                           "\n   Current Score: " + game.allTanks [2].InputList.Score +
			               "\n   Current Score: " + game.allTanks [3].InputList.Score +
                           "\n   Current Score: " + game.allTanks [4].InputList.Score +
                           "\n   Current Score: " + game.allTanks [5].InputList.Score +
                           "\n   Current Score: " + game.allTanks [6].InputList.Score +
                           "\n   Current Score: " + game.allTanks [7].InputList.Score +
			               "\n   Current Score: " + game.allTanks [8].InputList.Score +
                           "\n   Current Score: " + game.allTanks [9].InputList.Score +
                           "\n   Current Score: " + game.allTanks [10].InputList.Score +
                           "\n   Current Score: " + game.allTanks [11].InputList.Score +
                           "\n   Current Score: " + game.allTanks [12].InputList.Score +
                           "\n   Current Score: " + game.allTanks [13].InputList.Score +
                           "\n   Current Score: " + game.allTanks [14].InputList.Score +
                           "\n   Current Score: " + game.allTanks [15].InputList.Score +
                           "\n   Current Score: " + game.allTanks [16].InputList.Score +
                           "\n   Current Score: " + game.allTanks [17].InputList.Score +
                           "\n   Current Score: " + game.allTanks [18].InputList.Score +
                           "\n   Current Score: " + game.allTanks [19].InputList.Score;




				brief.text = "Current Generation: " + (game.round + 1) +"\nTime Left: "+Math.Round(30-game.currentRoundTime,2)+"\nLast Gen Highscore: " + game.highScore ;
			if(Input.GetKey(KeyCode.Tab)){
				panel.gameObject.SetActive (true);
			}
			else{
				panel.gameObject.SetActive (false);
			}
		}
	}
}