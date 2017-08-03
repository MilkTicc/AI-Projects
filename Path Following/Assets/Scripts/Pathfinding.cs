using UnityEngine;
using System.Collections;

using System.Collections.Generic;

namespace AISandbox {
    public class Pathfinding : MonoBehaviour {
        public Grid grid;
//		private GridNode StartNode;
//		private GridNode GoalNode;

		private int GRIDLENGTH = 32;
		private int GRIDHEIGHT = 20;
		private PriorityQueue<int, GridNode> Open = new PriorityQueue<int, GridNode>();
		private List<GridNode> Closed = new List<GridNode>();
        private void Awake() {
            // Create and center the grid
			grid.Create( GRIDHEIGHT,GRIDLENGTH );
            Vector2 gridSize = grid.size;
            Vector2 gridPos = new Vector2(gridSize.x * -0.5f, gridSize.y * 0.5f);
            grid.transform.position = gridPos;


        }

		private int heuristic(GridNode note1, GridNode note2){
			return Mathf.Abs(note1.row - note2.row) + Mathf.Abs(note1.column - note2.column);
		}

		private KeyValuePair<int,GridNode> inOpen(GridNode node){
			return Open.BaseHeap ().Find (item => item.Value == node);
		}


		public KeyValuePair < List<GridNode>, List<GridNode>> PathFindTheOne (PathFollowerController PFActor)
		{
			PFActor.StartNode.CostSoFar = 0;
			PFActor.StartNode.Heu = heuristic (PFActor.StartNode, PFActor.GoalNode);

			Open.Clear ();
			Closed.Clear ();
			Open.Enqueue (0, PFActor.StartNode);
			List<GridNode> thePath = new List<GridNode> ();
			List<GridNode> checkedNode = new List<GridNode> ();
			while (true) {
				GridNode currentNode = Open.DequeueValue ();

				if (currentNode == PFActor.GoalNode) {
					Closed.Clear ();
					while (Open.IsEmpty == false) {
						Open.DequeueValue ().SetPathStatus (GridNode.PathStatus._wasInOpen);
					}
					GridNode fromnode = PFActor.GoalNode;
					while (fromnode != PFActor.StartNode) {
						fromnode.SetPathStatus (GridNode.PathStatus.isOnPath);
						fromnode.DrawPath (PFActor.DrawPath);
						thePath.Add (fromnode);
						fromnode = fromnode.prevNode;
					}

					PFActor.StartNode.SetPathStatus (GridNode.PathStatus.isOnPath);
					
					thePath.Add (PFActor.StartNode);
					return new KeyValuePair< List<GridNode> ,List<GridNode>> (thePath, checkedNode);
					//break;
				}

				Closed.Add (currentNode);

				foreach (GridNode nextNode in currentNode.Neighbors) {

					if (nextNode._NodeStatus == GridNode.NodeStatus.blocked) {
						continue;
					}
					nextNode.SetPathStatus (GridNode.PathStatus._checked);

					checkedNode.Add (nextNode);
					int cost_so_far = heuristic (nextNode, currentNode) * nextNode.Cost + currentNode.CostSoFar;
					int heu = heuristic (nextNode, PFActor.GoalNode);
					if (nextNode.CostSoFar == 0) {
						nextNode.CostSoFar = cost_so_far;
					}
					if (nextNode.Heu == 0) {
						nextNode.Heu = heu;
					}


					if (inOpen (nextNode).Value != null && cost_so_far + heu < inOpen (nextNode).Key) {
						Open.Remove (inOpen (nextNode));
						Open.Enqueue (cost_so_far + heu, nextNode);
						nextNode.CostSoFar = cost_so_far;
						nextNode.Heu = heu;
						nextNode.prevNode = currentNode;
					}


					if (inOpen (nextNode).Value == null && !Closed.Contains (nextNode)) {
						Open.Enqueue (cost_so_far + heu, nextNode);
						nextNode.prevNode = currentNode;
					}

					if (nextNode != PFActor.GoalNode && nextNode != PFActor.StartNode) {
						nextNode.SetText ((nextNode.CostSoFar + nextNode.Heu).ToString ());
						nextNode._text.gameObject.SetActive (PathFollowing.debugging);
					}
				}
				if (Open.IsEmpty && currentNode!=PFActor.GoalNode) {
					List<GridNode> emptyList = new List<GridNode> ();
					emptyList.Add( PFActor.StartNode);
					return new KeyValuePair<List<GridNode>, List<GridNode>>(emptyList, null);
				}
			}
		}


		public KeyValuePair < List<GridNode>, List<GridNode>> PathfindNorm (PathFollowerController PFActor)
		{
			PFActor.StartNode.CostSoFar = 0;
			PFActor.StartNode.Heu = heuristic (PFActor.StartNode, PFActor.GoalNode);

			Open.Clear ();
			Closed.Clear ();
			Open.Enqueue (0, PFActor.StartNode);
			List<GridNode> thePath = new List<GridNode> ();
			List<GridNode> checkedNode = new List<GridNode> ();
			while (true) {
				GridNode currentNode = Open.DequeueValue ();

				if (currentNode == PFActor.GoalNode) {
					Closed.Clear ();

					GridNode fromnode = PFActor.GoalNode;
					while (fromnode != PFActor.StartNode) {
						
						fromnode.DrawPath (PFActor.DrawPath);
						thePath.Add (fromnode);
						fromnode = fromnode.prevNode;
					}

					thePath.Add (PFActor.StartNode);
					return new KeyValuePair< List<GridNode> ,List<GridNode>> (thePath, checkedNode);
					//break;
				}

				Closed.Add (currentNode);

				foreach (GridNode nextNode in currentNode.Neighbors) {

					if (nextNode._NodeStatus == GridNode.NodeStatus.blocked) {
						continue;
					}

					checkedNode.Add (nextNode);
					int cost_so_far = heuristic (nextNode, currentNode) * nextNode.Cost + currentNode.CostSoFar;
					int heu = heuristic (nextNode, PFActor.GoalNode);
					if (nextNode.CostSoFar == 0) {
						nextNode.CostSoFar = cost_so_far;
					}
					if (nextNode.Heu == 0) {
						nextNode.Heu = heu;
					}


					if (inOpen (nextNode).Value != null && cost_so_far + heu < inOpen (nextNode).Key) {
						Open.Remove (inOpen (nextNode));
						Open.Enqueue (cost_so_far + heu, nextNode);
						nextNode.CostSoFar = cost_so_far;
						nextNode.Heu = heu;
						nextNode.prevNode = currentNode;
					}


					if (inOpen (nextNode).Value == null && !Closed.Contains (nextNode)) {
						Open.Enqueue (cost_so_far + heu, nextNode);
						nextNode.prevNode = currentNode;
					}


				}
				if (Open.IsEmpty && currentNode!=PFActor.GoalNode) {
					List<GridNode> emptyList = new List<GridNode> ();
					emptyList.Add( PFActor.StartNode);
					return new KeyValuePair<List<GridNode>, List<GridNode>>(emptyList, null);
				}
			}
		}
    }
}