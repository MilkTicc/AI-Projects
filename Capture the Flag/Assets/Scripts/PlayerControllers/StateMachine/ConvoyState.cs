using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AISandbox
{
	public class ConvoyState : IStates
	{
		readonly OrientedActor self;
		Grid grid;
		GridNode goalNode;
		GridNode tarNode;
		CapturetheFlag CtF;
		Vector2 steering = Vector2.zero;
		List<GridNode> thePath;
		Pathfinding pathFinder;
		OrientedActor teamMate;

		public ConvoyState(OrientedActor actor, OrientedActor teammate){
			self = actor;
			pathFinder = self.pathFind;
			grid = self.grid;
			CtF = self.CtF;
			teamMate = teammate;
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

		}

		private void PathFollow (List<GridNode> path)
		{
			SeekToGrid (tarNode);
			if (self.currentNode == tarNode) {
				ResetPathColor ();
				FindPath ();
			}
			if (self.currentNode == goalNode) {
				if (self._Team == OrientedActor.Team.cyan){
					self.PrintMessage ("Status: Teammate Rescued!");
				}
				self.SetJob(1);
				teamMate.captured = false;
				teamMate.SetJob (2);

			}
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
			if (self._Team == OrientedActor.Team.cyan) {
				goalNode = CtF.cyanConvoyPoint;

			}
			if (self._Team == OrientedActor.Team.mangetta)
				goalNode = CtF.mangettaConvoyPoint;
			FindPath ();
		}
		public void Exit () { }
		public void Update ()
		{
			PathFollow (thePath);
		}
	}
}