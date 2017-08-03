using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace AISandbox
{
	public class UI : MonoBehaviour
	{
		public Text brief;
		public GameObject panel;
		public Text detail;
		public Text detail2;
		public Text hint;
		public TankGame game;
		public Graph graph;

		// Use this for initialization
		void Start ()
		{
			
			brief.transform.position = new Vector2 (Screen.width * 0.97f, Screen.height * 0.97f);
			detail.transform.position = new Vector2 (Screen.width * 0.03f, Screen.height * 0.45f);
			detail2.transform.position = new Vector2 (Screen.width * 0.17f, Screen.height * 0.45f);
			hint.transform.position = new Vector2 (Screen.width * 0.03f, Screen.height * 0.97f);
			graph.transform.position = new Vector2 (Screen.width * 0.47f, Screen.height * 0.43f);
			panel.GetComponent<Renderer> ().sortingLayerName = "UI";
			panel.GetComponent<Renderer> ().sortingOrder = 1;
		}

		// Update is called once per frame
		void Update ()
		{
			hint.text = "Hold <color=yellow>TAB</color> to Show Details and History Graph"+
				"\nPress <color=yellow>Space</color> to Toggle Debug Lines"+
				"\nCross Over Rate: "+Math.Round (game.CrossOverRate*100,0) + "%"+"  (Press <color=yellow>+ -</color> to change)"+
				"\nMutation Rate: "+ Math.Round(game.MutationRate*100,0)+"%"+ "  (Press <color=yellow>[ ] </color>to change)" ;
			
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

			detail2.text = "\n   Current Score: " + game.allTanks [0].MoveList.Score +
						   "\n   Current Score: " + game.allTanks [1].MoveList.Score +
                           "\n   Current Score: " + game.allTanks [2].MoveList.Score +
			               "\n   Current Score: " + game.allTanks [3].MoveList.Score +
                           "\n   Current Score: " + game.allTanks [4].MoveList.Score +
                           "\n   Current Score: " + game.allTanks [5].MoveList.Score +
                           "\n   Current Score: " + game.allTanks [6].MoveList.Score +
                           "\n   Current Score: " + game.allTanks [7].MoveList.Score +
			               "\n   Current Score: " + game.allTanks [8].MoveList.Score +
                           "\n   Current Score: " + game.allTanks [9].MoveList.Score +
                           "\n   Current Score: " + game.allTanks [10].MoveList.Score +
                           "\n   Current Score: " + game.allTanks [11].MoveList.Score +
                           "\n   Current Score: " + game.allTanks [12].MoveList.Score +
                           "\n   Current Score: " + game.allTanks [13].MoveList.Score +
                           "\n   Current Score: " + game.allTanks [14].MoveList.Score +
                           "\n   Current Score: " + game.allTanks [15].MoveList.Score +
                           "\n   Current Score: " + game.allTanks [16].MoveList.Score +
                           "\n   Current Score: " + game.allTanks [17].MoveList.Score +
                           "\n   Current Score: " + game.allTanks [18].MoveList.Score +
                           "\n   Current Score: " + game.allTanks [19].MoveList.Score;




			brief.text = "Current Generation: " + (game.round + 1) +
				//"\nTime Left: " + Math.Round (game.roundTime - game.currentRoundTime, 2) +
				"\nTime Left: "+ String.Format("{0:0.00}",game.roundTime - game.currentRoundTime) +
				"\nCurrent Highscore: " + game.currentRoundHS+
				"\nCurrent Total Score: " + game.totalScore+
				"\nCurrent Average Score: " + String.Format("{0:0.0}",(float)game.totalScore / game.tankCount);
			
			if(Input.GetKey(KeyCode.Tab)){
				panel.gameObject.SetActive (true);
			}
			else{
				panel.gameObject.SetActive (false);
			}
		}
	}
}