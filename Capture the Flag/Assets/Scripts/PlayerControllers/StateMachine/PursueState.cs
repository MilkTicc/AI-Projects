using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox
{
	public class PursueState : IStates
	{
		readonly OrientedActor self;
		OrientedActor enemy;
		Grid grid;
		Pathfinding pathFinder;
		GridNode goalNode;
		GridNode tarNode;
		CapturetheFlag CtF;
		Vector2 steering = Vector2.zero;
		List<GridNode> thePath;
		Defender defenderself;

		public PursueState (OrientedActor actor, Defender job, OrientedActor theenemy)
		{
			self = actor;
			enemy = theenemy;
			pathFinder = self.pathFind;
			grid = self.grid;
			CtF = self.CtF;
			defenderself = job;
		}

		float DisToGrid (GridNode node)
		{
			return (self.Position - (Vector2)node.transform.position).magnitude;
		}

		private void SeekToGrid (GridNode tarnode)
		{
			Vector2 pos = self.transform.position;
			Vector2 tarpos = tarnode.transform.position;
			Vector2 posdif = tarpos - pos;
			steering = (posdif.normalized * self.MaxSpeed / grid.PostoNode (self.Position).Cost - self.Velocity) * 50f;
			self.SetAcc (steering.x, steering.y);
		}

		private void ArriveAtGrid (GridNode tarnode)
		{
			Vector2 pos = self.Position;
			Vector2 tarpos = tarnode.transform.position;
			Vector2 posdif = tarpos - pos;
			float T = posdif.magnitude / 3f;
			steering = (posdif.normalized * self.MaxSpeed * T - self.Velocity) * 50f;
			self.SetAcc (steering.x, steering.y);
			//if (posdif.magnitude < 0.1f) {
			//	 //ForwardMessage();
			//	//Debug.Log (tFC.Name+": arrived at" + goalNode);
			//}
		}
		private void PathFollow (List<GridNode> path)
		{
			
				SeekToGrid (tarNode);
			if (self.currentNode == tarNode) {
				ResetPathColor ();
				FindPath ();
			}

			//if (tarNode == path [0]) {
			//	ArriveAtGrid (path [0]);
			//	if (DisToGrid (tarNode) < 0.5f) {
			//		ResetPathColor ();
			//		Enter ();
			//	}
			//}
		}

		public void FindPath ()
		{
			goalNode = enemy.currentNode;
			thePath = pathFinder.FindPath (self, goalNode);
			if (thePath.Count >= 2)
				tarNode = thePath [thePath.Count - 2];
			else
				tarNode = thePath [0];
			if (self._Team == OrientedActor.Team.cyan) {
				for (int i = 0; i < thePath.Count - 1; i++)
					thePath [i].GetComponent<SpriteRenderer> ().color += new Color (-0.3f, 0, 0);
			}
			//else{
			//	for (int i = 0; i < thePath.Count - 1; i++)
			//		thePath [i].GetComponent<SpriteRenderer> ().color += new Color (0, -0.3f, 0);
			//}
		}

		public void ResetPathColor ()
		{
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
			if (self._Team == OrientedActor.Team.cyan)
				{self.PrintMessage ("Status: Enemy Spotted!");
				self.PrintMessage ("Intent: Arrest the enemy");
			}
			FindPath ();
		}
		public void Exit () { }
		public void Update ()
		{
			PathFollow (thePath);
			if ((self.transform.position - enemy.transform.position).magnitude < 1.2f)
			{
				ResetPathColor ();
				enemy.currentJob.Captured (self);
				self.teamFlag.FlagSave ();

				if(self._Team == OrientedActor.Team.cyan)
				{self.PrintMessage ("Status: Enemy Captured");
					self.PrintMessage ("Intent: Escort Enemy to Jail");
				}

				self.currentJob.CurrentState = new EscortState (self, defenderself, enemy);
				self.currentJob.CurrentState.Enter ();
			}

			else if (enemy.captured == true)
				{self.currentJob.CurrentState = defenderself.patrolState;
				self.currentJob.CurrentState.Enter ();
			}
		}
	}
}