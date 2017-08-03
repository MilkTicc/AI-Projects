using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AISandbox
{
	public class MoveTo : Leaf
	{

		Dictionary<string, object> dB;
		TreasureFinderController tfc;
		Pathfinding pf;
		List<GridNode> Path = new List<GridNode>();
		GridNode Goal;
		//Image image;

		public MoveTo (Dictionary<string, object> db, string goal, Image asd)
		{
			dB = db;
			tfc = db ["TFC"] as TreasureFinderController;
			pf = tfc.pathfinder;
			image = asd;
			if (goal == "GK")
				Goal = dB ["GKNode"] as GridNode;
			else if (goal == "GD")
				Goal = dB ["GDNode"] as GridNode;
			else if (goal == "RK")
				Goal = dB ["RKNode"] as GridNode;
			else if (goal == "RD")
				Goal = dB ["RDNode"] as GridNode;
			else if (goal == "BK")
				Goal = dB ["BKNode"] as GridNode;
			else if (goal == "BD")
				Goal = dB ["BDNode"] as GridNode;
			else if (goal == "TS")
				Goal = dB ["TSNode"] as GridNode;
			Path = null;
			name = "MoveTo" + goal;
		}

		public override BehaviorTree.Node Init (Dictionary<string, object> dataContext)
		{
			//base.Init (dataContext);

			return this;
		}

		public void FindPath(){
			tfc.grid.ResetGrid();
			tfc.GoalNode = Goal;
			Path = pf.PathFindTheOne (tfc).Key;
			tfc.CheckedNode = pf.PathFindTheOne (tfc).Value;
			tfc.tarNode = Path [Path.Count - 1];
			Debug.Log (name);
			
		}

		public override BehaviorTree.Result Process ()
		{
			image.color = new Color (1, 1, 0, image.color.a);
			if (!pf.CheckAvailability (tfc.StartNode, Goal)) {
				image.color = new Color (1, 0, 0, image.color.a);
				return BehaviorTree.Result.FAILURE;
			}
			
			if (Path == null)
				FindPath();

			tfc.PathFollow (Path);

			if ((tfc._actor.Position - (Vector2)tfc.GoalNode.transform.position).magnitude < 0.1f) {
				tfc.ResetChecked ();
				image.color = new Color (0, 1, 0, image.color.a);
				return BehaviorTree.Result.SUCCESS;
			}
		return BehaviorTree.Result.RUNNING;
		}
	}
}