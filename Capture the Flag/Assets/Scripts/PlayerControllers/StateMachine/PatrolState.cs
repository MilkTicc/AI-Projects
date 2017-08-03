using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox
{
	public class PatrolState : IStates
	{
		readonly OrientedActor self;
		Grid grid;
		Pathfinding pathFinder;
		GridNode goalNode;
		GridNode tarNode;
		CapturetheFlag CtF;
		Vector2 steering = Vector2.zero;
		List<GridNode> thePath;
		Defender defenderself;
		//OrientedActor enemies;


		void SetRandGoal(){
			if (self._Team == OrientedActor.Team.cyan)
				goalNode = CtF.CGuardZone [Random.Range (0, CtF.CGuardZone.Count - 1)];
			if (self._Team == OrientedActor.Team.mangetta)
				goalNode = CtF.MGuardZone [Random.Range (0, CtF.MGuardZone.Count - 1)];
		}

		public PatrolState (OrientedActor actor, Defender job){
			self = actor;
			pathFinder = self.pathFind;
			grid = self.grid;
			CtF = self.CtF;
			defenderself = job;
			SetRandGoal ();
		}

		float DisToGrid (GridNode node)
		{
			return (self.Position - (Vector2)node.transform.position).magnitude;
		}

		private void SeekToGrid (GridNode tarnode)
		{
			Vector2 pos =self.transform.position;
			Vector2 tarpos = tarnode.transform.position;
			Vector2 posdif = tarpos - pos;
			steering = (posdif.normalized * self.MaxSpeed / grid.PostoNode (self.Position).Cost - self.Velocity) * 50f;
			self.SetAcc (steering.x, steering.y);
		}

		private void ArriveAtGrid(GridNode tarnode){
			Vector2 pos = self.Position;
			Vector2 tarpos = tarnode.transform.position;
			Vector2 posdif = tarpos - pos;
			float T = posdif.magnitude / 3f;
			steering = (posdif.normalized * self.MaxSpeed* T - self.Velocity)*50f;
			self.SetAcc (steering.x, steering.y);
				//if (posdif.magnitude < 0.1f) {
				//	 //ForwardMessage();
				//	//Debug.Log (tFC.Name+": arrived at" + goalNode);
				//}
		}
		private void PathFollow (List<GridNode> path)
			{
				if (tarNode != path [0]) {
					SeekToGrid (tarNode);
					if (DisToGrid (tarNode) < 0.5f) {
					//Influence ();
						tarNode = path [path.IndexOf (tarNode) - 1];
					}
				}
				if (tarNode == path [0]) {
					ArriveAtGrid (path [0]);
					if (DisToGrid (tarNode) < 0.5f) {
						SetRandGoal ();
						ResetPathColor ();
					FindPath ();
					}
				}
			}

		public void FindPath(){
			thePath = pathFinder.FindPath (self, goalNode);
			tarNode = thePath [thePath.Count - 1];
			if (self._Team == OrientedActor.Team.cyan) {
				for (int i = 0; i < thePath.Count - 1; i++)
					thePath [i].GetComponent<SpriteRenderer> ().color += new Color (-0.3f,0,0);
				}
			//else{
			//	for (int i = 0; i < thePath.Count - 1; i++)
			//		thePath [i].GetComponent<SpriteRenderer> ().color += new Color (0, -0.3f, 0);
			//}
		}

		public void ResetPathColor(){
			if (self._Team == OrientedActor.Team.cyan) {
				for (int i = 0; i < thePath.Count - 1; i++)
					thePath [i].GetComponent<SpriteRenderer> ().color += new Color (0.3f, 0, 0);
			}
			//else{
			//	for (int i = 0; i < thePath.Count - 1; i++)
			//		thePath [i].GetComponent<SpriteRenderer> ().color += new Color (0, 0.3f, 0);

			//}
		}

		public void Enter ()
		{
			//Debug.Log ("patrolling");
			if (self._Team == OrientedActor.Team.cyan)
				self.PrintMessage ("Intent: Guard the Treasure");
			FindPath ();
		}
		public void Exit(){}
		public void Update ()
		{
			
			PathFollow (thePath);
			foreach (OrientedActor enemy in self.enemies){
				//Debug.Log (grid.heuristic(enemy.currentNode, self.currentNode));
				//if((self._Team == OrientedActor.Team.cyan && enemy.currentNode.column < 15)||(self._Team == OrientedActor.Team.mangetta && enemy.currentNode.column > 15))
				if (grid.heuristic (enemy.currentNode, self.currentNode) <= 5 && !enemy.captured) {
					ResetPathColor ();
					defenderself.CurrentState = new PursueState (self, defenderself, enemy);
					defenderself.CurrentState.Enter ();}
			}

		}
	}
}