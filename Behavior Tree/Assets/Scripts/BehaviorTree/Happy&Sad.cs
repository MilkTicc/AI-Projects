using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AISandbox
{
	public class Happy : Leaf
	{
		//Image image;
		TreasureFinderController tfc;
		float COLOR_RANGE = 1f;

		public Happy(Dictionary<string, object> db,Image asd){
			tfc = db ["TFC"] as TreasureFinderController;
			image = asd;
		}


		public override BehaviorTree.Result Process ()
		{ 
			
			tfc.GetComponent<SpriteRenderer>().color = new Color (Random.Range (0, COLOR_RANGE), Random.Range (0, COLOR_RANGE), Random.Range (0, COLOR_RANGE), 1.0f);
			image.color = new Color (1, 1, 0, image.color.a);
			
			return BehaviorTree.Result.RUNNING;
		}
	
	
	}

	public class Sad : Leaf
	{

		TreasureFinderController tfc;
		//Image image;
		public Sad (Dictionary<string, object> db, Image asd)
		{
			tfc = db ["TFC"] as TreasureFinderController;
			image = asd;
		}


		public override BehaviorTree.Result Process ()
		{

			tfc.GetComponent<SpriteRenderer> ().color = new Color (0,0,0, 1.0f);
			image.color = new Color (1, 1, 0, image.color.a); 
			return BehaviorTree.Result.RUNNING;
		}


	}
}