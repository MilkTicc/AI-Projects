using UnityEngine;
using System.Collections.Generic;

namespace AISandbox {
    public class Pathfinding : MonoBehaviour {
        public Grid grid;
//		private GridNode StartNode;
//		private GridNode GoalNode;

		private PriorityQueue<int, GridNode> Open = new PriorityQueue<int, GridNode>();
		private List<GridNode> Closed = new List<GridNode>();
//		public TreasureFinderController _tfc;

		private int heuristic(GridNode note1, GridNode note2){
			return Mathf.Abs(note1.row - note2.row) + Mathf.Abs(note1.column - note2.column);
		}

		private KeyValuePair<int,GridNode> inOpen(GridNode node){
			return Open.BaseHeap ().Find (item => item.Value == node);
		}


		public List<GridNode> FindPath (OrientedActor actor, GridNode goal)
		{
			//Debug.Log ("calculating");
			GridNode startNode = actor.currentNode;
			startNode.CostSoFar = 0;
			startNode.Heu = heuristic (startNode, goal);
			Open.Clear ();
			Closed.Clear ();
			Open.Enqueue (0, startNode);
			List<GridNode> thePath = new List<GridNode> ();
			while (true) {
				GridNode currentNode = Open.DequeueValue ();

				if (currentNode == goal) {
					Closed.Clear ();
					while (Open.IsEmpty == false) {
						Open.DequeueValue ();
					}
					GridNode fromnode = goal;
					while (fromnode != startNode) {
						//fromnode.SetPathStatus (GridNode.PathStatus.isOnPath);
						//fromnode.DrawPath (PFActor.DrawPath);
						thePath.Add (fromnode);
						fromnode = fromnode.prevNode;
					}

					//startNode.SetPathStatus (GridNode.PathStatus.isOnPath);

					thePath.Add (startNode);
					return thePath;
					//break;
				}

				Closed.Add (currentNode);
				//Debug.Log(PFActor.toDoor);
				foreach (GridNode nextNode in currentNode.Neighbors) {

					if (nextNode._NodeStatus == GridNode.NodeStatus.blocked) {
						continue;
					}

					//nextNode.SetPathStatus (GridNode.PathStatus._checked);
					//checkedNode.Add (nextNode);
					int influence = actor._Team == OrientedActor.Team.cyan ? nextNode.MangInfluence : nextNode.CyanInfluence;
					int cost_so_far = heuristic (nextNode, currentNode) * (nextNode.Cost + influence) + currentNode.CostSoFar;
					int heu = heuristic (nextNode, goal);
					if (nextNode.CostSoFar == 0 && nextNode != startNode) {
						nextNode.CostSoFar = cost_so_far;
					}
					if (nextNode.Heu == 0 && nextNode != startNode) {
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

					//nextNode.SetText ((nextNode.CostSoFar + nextNode.Heu).ToString ());
					//nextNode._text.gameObject.SetActive (true);
					//if (nextNode != goal && nextNode != startNode) {
					//}
				}
				if (Open.IsEmpty && currentNode!=goal) {
					List<GridNode> emptyList = new List<GridNode> ();
					emptyList.Add( startNode);
					return emptyList;
				}
			}
		}

		//public bool CheckAvailability (TreasureFinderController tfc, GridNode tarNode)	
		//{

		//	GridNode StartNode = grid.PostoNode (new Vector2 (tfc.transform.position.x, tfc.transform.position.y));
		//	StartNode.CostSoFar = 0;
		//	StartNode.Heu = heuristic (StartNode, tarNode);
		//	Open.Clear ();
		//	Closed.Clear ();
		//	Open.Enqueue (0, StartNode);

		//	while(true){

		//		GridNode currentNode = Open.DequeueValue ();
		//		if(currentNode == tarNode){
		//			Closed.Clear ();
		//			return true;
		//		}

		//		Closed.Add (currentNode);

		//		foreach(GridNode nextNode in currentNode.Neighbors){

		//			if(nextNode._NodeStatus == GridNode.NodeStatus.blocked ){
		//				continue;
		//			}

		//			if(TreasureFind.ShutDoor.Contains(nextNode) && nextNode != tarNode){
		//				continue;
		//			}

		//			int cost_so_far = heuristic (nextNode, currentNode) * nextNode.Cost + currentNode.CostSoFar;
		//			int heu = heuristic (nextNode, tarNode);
		//			if (nextNode.CostSoFar==0){
		//				nextNode.CostSoFar = cost_so_far;
		//			}
		//			if(nextNode.Heu==0){
		//				nextNode.Heu = heu;
		//			}
		//			if(inOpen(nextNode).Value!=null && cost_so_far + heu < inOpen(nextNode).Key){
		//				Open.Remove (inOpen (nextNode));
		//				Open.Enqueue (cost_so_far + heu, nextNode);
		//				nextNode.CostSoFar = cost_so_far;
		//				nextNode.Heu = heu;
		//			}

		//			if(inOpen(nextNode).Value == null && !Closed.Contains(nextNode)){
		//				Open.Enqueue (cost_so_far + heu, nextNode);
		//			}
		//		}

		//		if(Open.IsEmpty && currentNode!=tarNode){
		//			return false;
		//		}

		//	}


		//}

		//public KeyValuePair < List<GridNode>, List<GridNode>> PathFindTheOne (TreasureFinderController PFActor)
		//{

		//	PFActor.StartNode.CostSoFar = 0;
		//	PFActor.StartNode.Heu = heuristic (PFActor.StartNode, PFActor.GoalNode);
		//	Open.Clear ();
		//	Closed.Clear ();
		//	Open.Enqueue (0, PFActor.StartNode);
		//	List<GridNode> thePath = new List<GridNode> ();
		//	List<GridNode> checkedNode = new List<GridNode> ();
		//	while (true) {
		//		GridNode currentNode = Open.DequeueValue ();

		//		if (currentNode == PFActor.GoalNode) {
		//			Closed.Clear ();
		//			while (Open.IsEmpty == false) {
		//				Open.DequeueValue ().SetPathStatus (GridNode.PathStatus._wasInOpen);
		//			}
		//			GridNode fromnode = PFActor.GoalNode;
		//			while (fromnode != PFActor.StartNode) {
		//				fromnode.SetPathStatus (GridNode.PathStatus.isOnPath);
		//				//fromnode.DrawPath (PFActor.DrawPath);
		//				thePath.Add (fromnode);
		//				fromnode = fromnode.prevNode;
		//			}

		//			PFActor.StartNode.SetPathStatus (GridNode.PathStatus.isOnPath);

		//			thePath.Add (PFActor.StartNode);
		//			return new KeyValuePair< List<GridNode> ,List<GridNode>> (thePath, checkedNode);
		//			//break;
		//		}

		//		Closed.Add (currentNode);
		//		//Debug.Log(PFActor.toDoor);
		//		foreach (GridNode nextNode in currentNode.Neighbors) {

		//			if (nextNode._NodeStatus == GridNode.NodeStatus.blocked) {
		//				continue;
		//			}
		//			if (TreasureFind.ShutDoor.Contains (nextNode) && nextNode != PFActor.GoalNode) {
		//				continue;
		//			}


		//			nextNode.SetPathStatus (GridNode.PathStatus._checked);

		//			checkedNode.Add (nextNode);
		//			int cost_so_far = heuristic (nextNode, currentNode) * nextNode.Cost + currentNode.CostSoFar;
		//			int heu = heuristic (nextNode, PFActor.GoalNode);
		//			if (nextNode.CostSoFar == 0 && nextNode != PFActor.StartNode) {
		//				nextNode.CostSoFar = cost_so_far;
		//			}
		//			if (nextNode.Heu == 0 && nextNode != PFActor.StartNode) {
		//				nextNode.Heu = heu;
		//			}


		//			if (inOpen (nextNode).Value != null && cost_so_far + heu < inOpen (nextNode).Key) {
		//				Open.Remove (inOpen (nextNode));
		//				Open.Enqueue (cost_so_far + heu, nextNode);
		//				nextNode.CostSoFar = cost_so_far;
		//				nextNode.Heu = heu;
		//				nextNode.prevNode = currentNode; 
		//			}


		//			if (inOpen (nextNode).Value == null && !Closed.Contains (nextNode)) {
		//				Open.Enqueue (cost_so_far + heu, nextNode);
		//				nextNode.prevNode = currentNode;
		//			}

		//			//nextNode.SetText ((nextNode.CostSoFar + nextNode.Heu).ToString ());
		//			//nextNode._text.gameObject.SetActive (true);
		//			//if (nextNode != PFActor.GoalNode && nextNode != PFActor.StartNode) {
		//			//}
		//		}
		//		if (Open.IsEmpty && currentNode!=PFActor.GoalNode) {
		//			List<GridNode> emptyList = new List<GridNode> ();
		//			emptyList.Add( PFActor.StartNode);
		//			return new KeyValuePair<List<GridNode>, List<GridNode>>(emptyList, null);
		//		}
		//	}
		//}

	}
}