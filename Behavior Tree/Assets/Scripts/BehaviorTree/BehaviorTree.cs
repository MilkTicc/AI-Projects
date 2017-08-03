
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AISandbox
{
	public class BehaviorTree
	{
		public enum Result
		{
			FAILURE,
			SUCCESS,
			RUNNING
		}

		public abstract class Node
		{
			public string name;
			public virtual Node Init (Dictionary<string, object> dataContext) { return this; }
			public abstract Result Process ();
			public Image image;
			public Image GetImage (){
					return this.image;

			}


		}

		private Dictionary<string, object> _dataContext;
		private Node _rootNode;

		public BehaviorTree (Node rootNode, Dictionary<string, object> db)
		{
			_rootNode = rootNode;
			_dataContext = db;
		}

		public Node Root()
		{
			return this._rootNode;
		}

		public void Init ()
		{
			_rootNode.Init (_dataContext);
		}

		public Result Process ()
		{
			return _rootNode.Process ();
		}
	}

	public abstract class Composite : BehaviorTree.Node
	{
		public List<BehaviorTree.Node> Children;
	}

	public abstract class Decorator : BehaviorTree.Node
	{
		public BehaviorTree.Node Child;
	}
	public abstract class Leaf : BehaviorTree.Node { }

	public class Sequence : Composite {
		int currentLeafIndex = 0;
		//Image image;
		public Sequence(Image asd){
			Children = new List<BehaviorTree.Node> ();
			image = asd;
		}

		//public void AddChildren(BehaviorTree.Node a){
		//	Children.Add (a);
		//	Debug.Log (Children.Count);
		//}

		public override BehaviorTree.Result Process(){
			image.color = new Color (1, 1, 0, image.color.a);
			
			if (currentLeafIndex == Children.Count) {
				currentLeafIndex = 0;
				image.color = new Color (0, 1, 0, image.color.a);
				
				return BehaviorTree.Result.SUCCESS;
			}
			
			BehaviorTree.Result status = this.Children [currentLeafIndex].Process ();

			if (status == BehaviorTree.Result.SUCCESS)
				currentLeafIndex += 1;
			if (status == BehaviorTree.Result.FAILURE) {
				image.color = new Color (1, 0, 0, image.color.a);

				return BehaviorTree.Result.FAILURE;
			}

			return status;
		}
	}

	public class Selector : Composite {
		int currentLeafIndex = 0;
		//Image image;
		public Selector(Image asd){
			
			Children = new List<BehaviorTree.Node> ();
			image = asd;		}

		public override BehaviorTree.Result Process ()
		{
			image.color = new Color (1, 1, 0, image.color.a);
			
			if (currentLeafIndex == Children.Count) {
				image.color = new Color (1, 0, 0,image.color.a);
				return BehaviorTree.Result.FAILURE;
			}
			
			BehaviorTree.Result status = this.Children [currentLeafIndex].Process ();

			if (status == BehaviorTree.Result.FAILURE)
				currentLeafIndex += 1;

			if(status == BehaviorTree.Result.SUCCESS){
				currentLeafIndex = 0;
				image.color = new Color (0, 1, 0,image.color.a);
				
				return BehaviorTree.Result.SUCCESS;
			}
		    return status;
		}

	}

	public class PriorityRepeater : Composite{
		int currentLeafIndex = 0;
		//Image image;
		public PriorityRepeater(Image asd){
			Children = new List<BehaviorTree.Node> ();
			image = asd;
		}
		public override BehaviorTree.Result Process (){
			image.color = new Color (1, 1, 0, image.color.a);

			if (currentLeafIndex == Children.Count) {
				image.color = new Color (1, 0, 0, image.color.a);
				return BehaviorTree.Result.FAILURE;
			}

			BehaviorTree.Result status = this.Children [currentLeafIndex].Process ();

			if (status == BehaviorTree.Result.FAILURE) {
				currentLeafIndex += 1;
				//Debug.Log (currentLeafIndex);
			}

			if (status == BehaviorTree.Result.SUCCESS) {
				if (currentLeafIndex != 0) {
					currentLeafIndex = 0;
					foreach (Sequence child in Children){
						child.GetImage ().color = new Color (0, 0, 1, child.GetImage ().color.a );
						//child.Children [0].image.color = new Color (0, 0, 1, child.Children [0].image.color.a);
						//child.Children[1].image.color = new Color (0, 0, 1, child.Children [1].image.color.a);
						//Debug.Log ("asd");
					}
					//Debug.Log (currentLeafIndex);
				} else {
					currentLeafIndex = 0;
					image.color = new Color (0, 1, 0, image.color.a);
					return BehaviorTree.Result.SUCCESS;}
			}

			return BehaviorTree.Result.RUNNING;
		}
	}

	public class KeyCheck : Leaf{
		bool haveKey;
		Dictionary<string, object> dB;
		//Image image;
		string color;
		public KeyCheck (Dictionary<string, object> db, string c,Image asd)
		{
			image = asd;
			color = c;
			dB = db;

		}
			public override BehaviorTree.Result Process(){
			if (color == "green") {
				haveKey =  System.Convert.ToBoolean(dB ["haveGreenKey"]);
				//Debug.Log (haveKey);
				name = "GreenKeyCheck";
			} else if (color == "red") {
				haveKey = System.Convert.ToBoolean (dB ["haveRedKey"]);
				name = "RedKeyCheck";
			} else if (color == "blue") {
				haveKey = System.Convert.ToBoolean (dB ["haveBlueKey"]);
				name = "BlueKeyCheck";
			} else
				haveKey = false;

			if (haveKey) {
				//Debug.Log ("gkc:fail");
				image.color = new Color (1, 0, 0, image.color.a);
				return BehaviorTree.Result.FAILURE;
			}
			//Debug.Log ("gkc:fsuccess");
			Debug.Log ("key check success");
			
			image.color = new Color (0, 1, 0, image.color.a);
			return BehaviorTree.Result.SUCCESS;
			}


	}

	public class DoorCheck : Leaf
	{
		bool doorOpened;
		bool haveKey;
		Dictionary<string, object> dB;
		//Image image;
		string color;
		public DoorCheck (Dictionary<string, object> db, string c,Image asd)
		{
			image = asd;
			color = c;
			dB = db;

		}

		public override BehaviorTree.Result Process ()
		{
			if (color == "green") {
				haveKey = System.Convert.ToBoolean (dB ["haveGreenKey"]);
			} else if (color == "red") {
				haveKey = System.Convert.ToBoolean (dB ["haveRedKey"]);
			} else if (color == "blue") {
				haveKey = System.Convert.ToBoolean (dB ["haveBlueKey"]);
			} else
				haveKey = false;

			if (!haveKey) {
				image.color = new Color (1, 0, 0, image.color.a);
				return BehaviorTree.Result.FAILURE;
			}

			if (color == "green") {
				doorOpened = System.Convert.ToBoolean (dB ["openedGreenDoor"]);
				name = "GreenDoorCheck";
			} else if (color == "red") {
				doorOpened = System.Convert.ToBoolean (dB ["openedRedDoor"]);
				name = "RedDoorCheck";
			} else if (color == "blue") {
				doorOpened = System.Convert.ToBoolean (dB ["openedBlueDoor"]);
				name = "BlueDoorCheck";
			} else
				doorOpened = false;

			if (doorOpened) {
				image.color = new Color (1, 0, 0, image.color.a);
				return BehaviorTree.Result.FAILURE;
			}

			Debug.Log ("door check success");
			image.color = new Color (0, 1, 0, image.color.a);
			return BehaviorTree.Result.SUCCESS;
		}


	}

}