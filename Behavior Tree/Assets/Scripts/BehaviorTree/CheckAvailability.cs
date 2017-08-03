using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox
{
	public class CheckAvailability : Leaf
	{

		TreasureFinderController tfc;
		Pathfinding pf;
		bool available;

		public CheckAvailability (Dictionary<string, object> db, GridNode goal)
		{
			//dB = db;
			tfc = db ["TFC"] as TreasureFinderController;
			pf = tfc.pathfinder;
			tfc.GoalNode = goal;
			available = pf.CheckAvailability (tfc.grid.PostoNode (new Vector2 (tfc.transform.position.x, tfc.transform.position.y)), goal);
		}




		public override BehaviorTree.Result Process ()
		{
			if (available)
				return BehaviorTree.Result.SUCCESS;

			return BehaviorTree.Result.FAILURE;


		}
	}
}