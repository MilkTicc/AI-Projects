using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace AISandbox {
    public class Pathfinding : MonoBehaviour {
        public Grid grid;
		private GridNode StartNode;
		private GridNode GoalNode;
		public Text text;
		private int GRIDLENGTH = 32;
		private int GRIDHEIGHT = 18;
		private PriorityQueue<int, GridNode> Open = new PriorityQueue<int, GridNode>();
		private List<GridNode> Closed = new List<GridNode>();
        private void Start() {
            // Create and center the grid
			grid.Create( GRIDHEIGHT,GRIDLENGTH );
//			GoalNode = grid.SetGoal (5,5);
//			StartNode = grid.SetStart (15, 20);
			//grid.SetGoal(1,1);
            Vector2 gridSize = grid.size;
            Vector2 gridPos = new Vector2(gridSize.x * -0.5f, gridSize.y * 0.5f);
            grid.transform.position = gridPos;
//			text.text = "Hold <b><color=yellow>Left Shift</color></b> and left click to set the <b><color=yellow>Goal</color></b>, " +
//				"Hold <b><color=green>Left Control</color></b> and left click to set the <b><color=green>Starting Point</color></b>, " +
//				"Click '<b><color=red>R</color></b>' to reset";

			text.text = "                <b><color=yellow>Goal</color></b>: Left Shift and Left Mouse" +
				"          <b><color=green>Starting Point</color></b>: Left Control and Left Mouse" +
				"          <b><color=red>Reset</color></b>: Button R" +
				"          <b><color=blue>Block</color></b>: Left Mouse";
			text.transform.position = new Vector2 (350f,Screen.height-20f);
        }

		private int heuristic(GridNode note1, GridNode note2){
			return Mathf.Abs(note1.row - note2.row) + Mathf.Abs(note1.column - note2.column);
		}

		private KeyValuePair<int,GridNode> inOpen(GridNode node){
			return Open.BaseHeap ().Find (item => item.Value == node);
		}

		private void Pathfind(){
			grid.RefindPath ();
			StartNode.Cost = 0;
			StartNode.Heu = heuristic (StartNode, GoalNode);
			Open.Clear ();
			Closed.Clear ();
			Open.Enqueue(0,StartNode);

			while(true){
			//while (Open.IsEmpty == false && GoalNode !=null && StartNode != null) {
				GridNode currentNode = Open.DequeueValue();

				if (currentNode == GoalNode) {
					Closed.Clear ();
					while (Open.IsEmpty == false) {
						Open.DequeueValue().WasInOpen = true;
					}
					GridNode fromnode = GoalNode;
					while (fromnode != StartNode) {
						fromnode.isOnPath = true;
						fromnode = fromnode.prevNode;
					}
					StartNode.isOnPath = true;
					break;
				}

				//Debug.Log (currentNode.name);

				Closed.Add (currentNode);

				foreach (GridNode nextNode in currentNode.GetNeighbors()) {

					if (nextNode.blocked) {
						continue;
					}

					nextNode.ischecked = true;
					int cost = heuristic (nextNode, currentNode) + currentNode.Cost;
					int heu = heuristic (nextNode, GoalNode);
					if (nextNode.Cost == 0) {
						nextNode.Cost = cost;
					}
					if (nextNode.Heu == 0) {
						nextNode.Heu = heu;
					}

				//	KeyValuePair<int,GridNode > nextPair = new KeyValuePair<int,GridNode> (nextNode.Cost + heuristic (nextNode, GoalNode), nextNode);
					if (inOpen(nextNode).Value!=null  && cost + heu < inOpen(nextNode).Key) {
						Open.Remove (inOpen (nextNode));
						Open.Enqueue (cost + heu, nextNode);
						nextNode.Cost = cost;
						nextNode.Heu = heu;
						nextNode.prevNode = currentNode;
					}


					if (inOpen(nextNode).Value==null && !Closed.Contains (nextNode)){
						Open.Enqueue (cost + heu, nextNode);
						nextNode.prevNode = currentNode;
					}

					if(nextNode.isGoal ==false && nextNode.isStart==false)
					nextNode.SetText ((nextNode.Cost + nextNode.Heu).ToString());
				}
				if (Open.IsEmpty) {
					//print("There is no path.");
					grid.nopath = true;
					break;
				}

			}

		
		}


		private void Update(){
			GoalNode = grid.getGoalNode;
			StartNode = grid.getStartNode;

			if (grid.newpath == true && StartNode != null && GoalNode !=null) {
				
				Pathfind ();
				//print("finding new path");
				grid.newpath = false;
			}

			if (Input.GetKeyDown (KeyCode.R)) {
				grid.Restart ();
			}
		}

    }
}