using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox
{
	public class ReturnState : IStates
	{
		readonly OrientedActor self;
		Grid grid;
		Pathfinding pathFinder;
		GridNode goalNode;
		GridNode tarNode;
		CapturetheFlag CtF;
		Vector2 steering = Vector2.zero;
		List<GridNode> thePath;
		Flagger flagger;

		public ReturnState (OrientedActor actor,Flagger job)
		{
			self = actor;
			pathFinder = self.pathFind;
			grid = self.grid;
			CtF = self.CtF;
			flagger = job;
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
			//if (tarNode != path [0]) {
				SeekToGrid (tarNode);
			if (self.currentNode == tarNode) {
				//tarNode = path [path.IndexOf (tarNode) - 1];
				ResetPathColor ();	
				FindPath();
			}
			if(self.currentNode == goalNode)
			{
				if (self._Team == OrientedActor.Team.cyan)
					self.PrintMessage ("Status: We won!!!");
				else
					self.enemies[0].PrintMessage ("Status: We Lost!!!");

				self.currentJob.ToHappy ();
				foreach(OrientedActor player in self.teammates)
					player.currentJob.ToHappy ();
				foreach (OrientedActor player in self.enemies)
					player.currentJob.ToSad ();

			 }

			//}
			//if (tarNode == path [0]) {
			//	ArriveAtGrid (path [0]);
			//	if (DisToGrid (tarNode) < 0.5f) {
					
			//	}
			//}
		}

		public void FindPath ()
		{
			thePath = pathFinder.FindPath (self, goalNode);
			if (thePath.Count >= 2)
				tarNode = thePath [thePath.Count - 2];
			else
				tarNode = thePath [0];
			if (self._Team == OrientedActor.Team.cyan) {
				for (int i = 0; i < thePath.Count - 1; i++)
					thePath [i].GetComponent<SpriteRenderer> ().color += new Color (-0.3f, 0, 0);
			} else {
				for (int i = 0; i < thePath.Count - 1; i++)
					thePath [i].GetComponent<SpriteRenderer> ().color += new Color (0, -0.3f, 0);
			}
		}

		public void ResetPathColor ()
		{
			if (self._Team == OrientedActor.Team.cyan) {
				for (int i = 0; i < thePath.Count - 1; i++)
					thePath [i].GetComponent<SpriteRenderer> ().color += new Color (0.3f, 0, 0);
			} else {
				for (int i = 0; i < thePath.Count - 1; i++)
					thePath [i].GetComponent<SpriteRenderer> ().color += new Color (0, 0.3f, 0);

			}
		}

		public void Enter ()
		{
			if (self._Team == OrientedActor.Team.cyan)
			{
				//Debug.Log (CtF.mangettaFlag);
				goalNode = CtF.cyanReturnPoint; 
				
			}
			if (self._Team == OrientedActor.Team.mangetta)
				goalNode = CtF.mangettaReturnPoint;
			FindPath ();
		}
		public void Exit () { }
		public void Update ()
		{
			PathFollow (thePath);
		}
	}
}