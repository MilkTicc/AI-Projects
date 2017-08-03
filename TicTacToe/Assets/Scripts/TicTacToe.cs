using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AISandbox
{
	public class TicTacToe : MonoBehaviour
	{
		public Grid grid;
		int GRIDROWS = 6;
		int GRIDCOLS = 6;
		public int boardEvaluation;
		Color _highlight_color = new Color (1, 1, 0);
		Color _orig_color = new Color (0.8f, 0.8f, 0.8f);
		GridNode prevHighlitNode;
		GridNode prevNode;
		List<GridNode> playerNodes = new List<GridNode> ();
		List<GridNode> aiNodes = new List<GridNode> ();
		//List<GridNode>
		int playerScore;
		int aiScore;
		public int difficulty = 2;
		public bool playersTurn;
		bool gameEnd;
		public string gameResult;
		bool aICalculation;
		// Use this for initialization
		public int PlayerScore { get { return playerScore; } }
		public int AIScore{ get { return aiScore;}}
		GridNode aINode;
		public bool gameOver = false;
		void Start ()
		{

			grid.Create (GRIDROWS, GRIDCOLS);
			Vector2 gridSize = grid.size;
			Vector2 gridPos = new Vector2 (gridSize.x * -0.9f, gridSize.y * 0.5f);
			grid.transform.position = gridPos;
			ResetGame ();
		}

		void Highlight(){
			if (prevHighlitNode != null)
				prevHighlitNode._renderer.color = _orig_color;

			if(grid.MouseNode!=null && grid.MouseNode.NodeStatus == GridNode.NodeStat.open)
			{grid.MouseNode._renderer.color = _highlight_color;
				prevHighlitNode = grid.MouseNode;
			}
		}

		int MoveScoreCal(GridNode node, bool isPlayer){
			int ScoreIncrease = 0;
			int n = isPlayer ? 1 : 2;
			List<GridNode> checkingList = isPlayer ? playerNodes : aiNodes;
			if(node.GetNeighbors(n).Count != 0){
				foreach (GridNode neighbor in node.GetNeighbors (n))
				{
					int rowDiff = neighbor.row - node.row;
					int colDiff = neighbor.column - node.column;
					GridNode tarNode1 = grid.GetNode (node.row + 2 * rowDiff, node.column + 2 * colDiff);
					GridNode tarNode2 = grid.GetNode (node.row + 3*rowDiff, node.column + 3*colDiff);
					GridNode tarNode3 = grid.GetNode (node.row - 2 * rowDiff, node.column - 2 * colDiff);
					GridNode tarNode4 = grid.GetNode (node.row - rowDiff, node.column - colDiff);

					if (checkingList.Contains (tarNode1) && checkingList.Contains (tarNode2)) {
						ScoreIncrease++;
							//Debug.Log ("node: " + node.row + " " + node.column + " neighbor:" + neighbor.row + "" + neighbor.column + " straight");
						}
						if (checkingList.Contains (tarNode3) && checkingList.Contains (tarNode4)) {
							ScoreIncrease++;
							//Debug.Log ("node: " + node.row + " " + node.column + " neighbor:" + neighbor.row + "" + neighbor.column + " back");
						}
					}
				}
			return ScoreIncrease;
		}

		int ThreeInRowCal (GridNode node, bool isPlayer)
		{
			int threeInRows = 0;
			int n = isPlayer ? 1 : 2;
			List<GridNode> checkingList = isPlayer ? playerNodes : aiNodes;
			if (node.GetNeighbors (n).Count != 0) {
				foreach (GridNode neighbor in node.GetNeighbors (n)) {
					int rowDiff = neighbor.row - node.row;
					int colDiff = neighbor.column - node.column;
					GridNode tarNode1 = grid.GetNode (node.row + 2 * rowDiff, node.column + 2 * colDiff);
					GridNode tarNode4 = grid.GetNode (node.row - rowDiff, node.column - colDiff);

					if (checkingList.Contains (tarNode1)) {
						threeInRows++;
						//Debug.Log ("node: " + node.row + " " + node.column + " neighbor:" + neighbor.row + "" + neighbor.column + " straight");
					}
					if ( checkingList.Contains (tarNode4)) {
						threeInRows++;
						//Debug.Log ("node: " + node.row + " " + node.column + " neighbor:" + neighbor.row + "" + neighbor.column + " back");
					}
				}
			}
			return threeInRows;
		}

		IEnumerator AITurn(){
			//GridNode aINode = grid.AvaliableNodes [Random.Range (0, grid.AvaliableNodes.Count - 1)];
			//yield return new WaitForSeconds(1f);
			yield return null;
			//GridNode aINode;
			//aINode = MinMax (2, false).Key;
			Minmax (difficulty, false, int.MinValue, int.MaxValue, prevNode, out aINode);
			aiNodes.Add (aINode);
			aINode.NodeStatus = GridNode.NodeStat.cross;
			aINode.DrawNode ();
			aiScore += MoveScoreCal (aINode,false);
			boardEvaluation -= AccessMove (aINode, false);

			playersTurn = true;
			//Debug.Log ("end time:" + Time.time);

		}

		void PlayerTurn(){
			if (grid.MouseNode != prevHighlitNode)
				Highlight ();
			if (Input.GetMouseButtonDown (0) && grid.MouseNode && grid.MouseNode.NodeStatus == GridNode.NodeStat.open) {
				grid.MouseNode.NodeStatus = GridNode.NodeStat.circle;
				grid.MouseNode._renderer.color = _orig_color;
				grid.MouseNode.DrawNode ();
				playerNodes.Add (grid.MouseNode);
				prevNode = grid.MouseNode;
				playerScore += MoveScoreCal (grid.MouseNode, true);
				boardEvaluation += AccessMove (grid.MouseNode, true);
				playersTurn = false;
				aICalculation = false;
				//Debug.Log ("start time:" + Time.time);
			}
		}

		//KeyValuePair<GridNode,int> MinMax( int depth, bool isPlayer){
		//	GridNode currentNode = null;
		//	List<GridNode> Moves = grid.AvaliableNodes;
		//	//List<int> scores = new List<int> ();

		//	if (depth == 0) {
		//		foreach (GridNode nodes in grid.AvaliableNodes)
		//			if (currentNode == null || AccessMove (nodes, isPlayer) > AccessMove (currentNode, isPlayer))
		//				currentNode = nodes;
		//		return new KeyValuePair<GridNode, int> (currentNode, AccessMove (currentNode, isPlayer));
		//	}

		//	if(!isPlayer)
		//	{foreach (GridNode nodes in Moves) {
		//			//scores.Add (AccessMove (nodes, isPlayer));
		//			if (currentNode == null || -AccessMove (nodes, !isPlayer) + MinMax (depth - 1, isPlayer).Value < -AccessMove (currentNode, !isPlayer) + MinMax (depth - 1, isPlayer).Value)
		//				currentNode = nodes;
		//		}
		//		return new KeyValuePair<GridNode, int> (currentNode, AccessMove (currentNode, !isPlayer) - MinMax (depth - 1, isPlayer).Value);
		//	}

		//	else
		//		{
		//		foreach (GridNode nodes in Moves) {
		//			//scores.Add (AccessMove (nodes, isPlayer));
		//			if (currentNode == null || AccessMove (nodes, isPlayer) - MinMax (depth - 1, !isPlayer).Value > AccessMove (currentNode, isPlayer) - MinMax (depth - 1, !isPlayer).Value)
		//				currentNode = nodes;
		//		}
		//		return new KeyValuePair<GridNode, int> (currentNode, AccessMove (currentNode, isPlayer) - MinMax (depth - 1, !isPlayer).Value);
		//	}


		//}

		public int Minmax(int depth, bool isPlayer, int alpha, int beta,GridNode thenode, out GridNode aINode){
			aINode = null;
			List<GridNode>  moves = grid.AvaliableNodes;
			moves.Remove (thenode);
			if(depth == 1 || moves.Count == 0){
				foreach (GridNode node in grid.AvaliableNodes)
					if (aINode == null || AccessMove (node, isPlayer) > AccessMove (aINode, isPlayer))
						aINode = node;
				return AccessMove (aINode, isPlayer);
			}
			if (isPlayer) {
				int score = int.MinValue;
				foreach (GridNode node in moves) {
					score = Mathf.Max (score, Minmax (depth - 1, !isPlayer, alpha, beta, node, out aINode));
					alpha = Mathf.Max (alpha, score);
					if (beta <= alpha)
						break;
				}
				return score;
			} else
				{int score = int.MaxValue;
				foreach (GridNode node in moves) {
					score = Mathf.Min (score, Minmax (depth - 1, isPlayer, alpha, beta, node, out aINode));
					beta = Mathf.Min (beta, score);
					if (beta <= alpha)
						break;
				}
				return score;
			}

		}

		void ResetGame(){
			playersTurn = true;
			playerScore = 0;
			gameOver = false;
			playerNodes.Clear ();
			boardEvaluation = 0;
			aiScore = 0;
			aiNodes.Clear ();
			gameResult = "";
			foreach(GridNode nodes in grid.AllNode)
			{
				nodes.NodeStatus = GridNode.NodeStat.open;
				nodes.DrawNode ();
			}
		}

		void CalcResult(){
			if (playerScore > aiScore)
				gameResult = "<b>Player Won!</b>";
			else if (playerScore == aiScore)
				gameResult = "<b>Tie</b>";
			else
				gameResult = "<b>AI Won!</b>";
		}

		int AccessMove(GridNode move, bool isPlayer){
			int moveValue = 0;
			int n = isPlayer ? 1 : 2;
			GridNode.NodeStat ownStat = isPlayer ? GridNode.NodeStat.circle : GridNode.NodeStat.cross;
			GridNode.NodeStat enemyStat = isPlayer ? GridNode.NodeStat.cross : GridNode.NodeStat.circle;
			//if (move.row == 0 || move.row == GRIDCOLS - 1)
			//	moveValue += 1;
			if (move.row == 1 || move.row == GRIDCOLS - 2)
				moveValue += 1;
 			else if (move.row == 2 || move.row == GRIDCOLS - 3)
				moveValue += 2;

			//if (move.column == 0 || move.column == GRIDCOLS - 1)
			//	moveValue +=1;
			if (move.column == 1 || move.column == GRIDCOLS - 2)
				moveValue += 1;
			else if (move.column == 2 || move.column == GRIDCOLS - 3)
				moveValue += 2;

			if (MoveScoreCal (move, isPlayer) != 0)
				moveValue += MoveScoreCal (move, isPlayer) * 10;
			if (MoveScoreCal (move, !isPlayer) != 0)
				moveValue += MoveScoreCal (move, !isPlayer) * 8;
			if (ThreeInRowCal (move, isPlayer) != 0)
				moveValue += ThreeInRowCal (move, isPlayer) * 7;
			if (ThreeInRowCal (move, !isPlayer) != 0)
				moveValue += ThreeInRowCal (move, !isPlayer) * 5;
		
			foreach (GridNode neighbor in move.GetNeighbors (n))
				{if (neighbor.NodeStatus == ownStat)
					moveValue += 3;
				if (neighbor.NodeStatus == enemyStat)
					moveValue += 1;
			}
			//moveValue = isPlayer ? moveValue : -moveValue;
			return moveValue;
		}

		void Update ()
		{
			//if(grid.MouseNode)
			//	Debug.Log ("Row: " + grid.MouseNode.row + " Col: "+ grid.MouseNode.column + " Access: "+AccessMove(grid.MouseNode, true));

			if (Input.GetKeyDown (KeyCode.Space))
				ResetGame ();

			if(playersTurn)
			{
				PlayerTurn ();
				if (Input.GetKeyDown (KeyCode.Alpha1))
					difficulty = 1;
				if (Input.GetKeyDown (KeyCode.Alpha2))
					difficulty = 2;
				if (Input.GetKeyDown (KeyCode.Alpha3))
					difficulty = 3;
				if (Input.GetKeyDown (KeyCode.Alpha4))
					difficulty = 4;
				if (Input.GetKeyDown (KeyCode.Alpha5))
					difficulty = 5;
			}
			if(!playersTurn && !aICalculation){
				//AITurn ();
				//Debug.Log ("end time:" + Time.time);
				
				StartCoroutine(AITurn ());
				aICalculation = true;

				//Debug.Log (grid.AvaliableNodes.Count);
			}

			if(grid.AvaliableNodes.Count == 0)
			{
				gameOver = true;
				CalcResult ();
			}


		}
	}
}