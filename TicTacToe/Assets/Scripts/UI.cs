using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AISandbox
{
	public class UI : MonoBehaviour
	{
		public Text title;
		public Text playerScore;
		public Text aiScore;
		public TicTacToe game;
		public Text result;
		public Text turn;
		public Text evaluation;
		public Text diff;
		// Use this for initialization
		void Start ()
		{
			title.transform.position = new Vector2 (Screen.width * 0.77f, Screen.height * 0.91f);
			turn.transform.position = new Vector2 (Screen.width * 0.77f, Screen.height * 0.7f);
			result.transform.position = new Vector2 (Screen.width * 0.77f, Screen.height * 0.12f);
			playerScore.transform.position = new Vector2 (Screen.width * 0.8f, Screen.height * 0.5f);
			aiScore.transform.position = new Vector2 (Screen.width * 0.8f, Screen.height * 0.4f);
			evaluation.transform.position = new Vector2 (Screen.width * 0.77f, Screen.height * 0.25f);
			diff.transform.position = new Vector2 (Screen.width * 0.77f, Screen.height * 0.8f);

		}

		// Update is called once per frame
		void Update ()
		{
			playerScore.text = "Player Score: " + game.PlayerScore;
			aiScore.text = "A.I Score: " + game.AIScore;
			turn.text = game.gameOver? game.gameResult: (game.playersTurn? "Player's Turn \nPlease make your move": "A.I's Turn \nCalculating...");
			result.text = "Press <color=yellow><b>Space</b></color> to Restart the Game\nPress <color=yellow><b>1-5</b></color> to Change the Difficulty of the Game\n(MinMax Depth)";
			evaluation.text = "Board Evaluation: " + game.boardEvaluation;
			switch(game.difficulty)
			{case 1:
				diff.text = "Current Difficulty: <color=green>Easie Piesie</color>";
				break;
			case 2:
				diff.text = "Current Difficulty: <color=cyan>Not that Hard Either</color>";
				break;
			case 3:
				diff.text = "Current Difficulty: <color=yellow>Bring on the Challenge</color>";
				break;
			case 4:
				diff.text = "Current Difficulty: <color=#ff00ff>What took you so long</color>";
				break;
			case 5:
				diff.text = "Current Difficulty: <color=red>I really like waiting</color>";
				break;

			}


		}
	}
}