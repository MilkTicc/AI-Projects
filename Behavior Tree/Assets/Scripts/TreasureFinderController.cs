using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AISandbox{
	[RequireComponent(typeof (IActor))]
	public class TreasureFinderController : MonoBehaviour {
		public IActor _actor;
		public Pathfinding pathfinder;
		public Grid grid;
		public LineRenderer Steering_Line;
		public TreasureFind treasureFind;
		//private List<GridNode> Path = new List<GridNode>();
		public List<GridNode> CheckedNode = new List<GridNode> ();
		private Vector2 steering;
		private GridNode startNode;
		private GridNode goalNode;
		public GridNode tarNode;
		BehaviorTree behaviorT;
		[HideInInspector]
		public bool canGetGreenKey, canGetRedKey,canGetBlueKey,canGetGreenDoor,canGetBlueDoor,canGetRedDoor, canGetTreasure;
		[HideInInspector]
		public bool haveGreenKey=false, haveRedKey=false, haveBlueKey=false,haveTreasure=false , cantFindPath;
		public GridNode StartNode{get{return startNode;} set{startNode = value;}}
		public GridNode GoalNode{get{return goalNode;} set{goalNode = value;}}
		//GridNode greenKey, redKey, blueKey, greenDoor, blueDoor, redDoor, treaSure;
		private float DisToGrid (GridNode node)
		{return (_actor.Position - (Vector2)node.transform.position).magnitude;
		}

		private void SeekToGrid (GridNode tarnode)
		{
			Vector2 pos = _actor.Position;
			Vector2 tarpos = tarnode.transform.position;
			Vector2 posdif = tarpos - pos;
			steering = (posdif.normalized * _actor.MaxSpeed / grid.PostoNode (_actor.Position).Cost - _actor.Velocity) * 50f;
			_actor.SetAcc (steering.x, steering.y);
		}

		private void ArriveAtGrid(GridNode tarnode){
			Vector2 pos = _actor.Position;
			Vector2 tarpos = tarnode.transform.position;
			Vector2 posdif = tarpos - pos;
			float T = posdif.magnitude / 3f;
			steering = (posdif.normalized * _actor.MaxSpeed* T - _actor.Velocity)*50f;
			_actor.SetAcc (steering.x, steering.y);

		}

		public void PathFollow (List<GridNode> path)
		{
			
			if (tarNode != path [0]) {
				if (DisToGrid (tarNode) < 2f) {
					tarNode = path [path.IndexOf (tarNode) - 1];
				}
				if (tarNode != goalNode) {
					SeekToGrid (tarNode);
				}
			}
			if (tarNode == path [0]) {
				ArriveAtGrid (path [0]);
			}
		}

//		private void NewPath ()
//		{
//			startNode = grid.PostoNode (_actor.Position);
//			// goalNode = grid.RandomNode;
//			startNode.DrawNodes ();
//			goalNode.DrawNodes ();
//
//			Path = pathfinder.PathFindTheOne (this).Key;
//			CheckedNode = pathfinder.PathFindTheOne (this).Value;	
//
//			tarNode = Path [Path.Count -1];
//
//		}

		void AIBehavior(){
			//redKey = grid.GetNode ((int)grid.RedKeyPos.x, (int)grid.RedKeyPos.y);
			//greenKey = grid.GetNode ((int)grid.GreenKeyPos.x, (int)grid.GreenKeyPos.y);
			//blueKey = grid.GetNode ((int)grid.BlueKeyPos.x, (int)grid.BlueKeyPos.y);
			//greenDoor = grid.GetNode ((int)grid.GreenDoorPos.x, (int)grid.GreenDoorPos.y);
			//blueDoor = grid.GetNode ((int)grid.BlueDoorPos.x, (int)grid.BlueDoorPos.y);
			//redDoor = grid.GetNode ((int)grid.RedDoorPos.x, (int)grid.RedDoorPos.y);
			//treaSure = grid.GetNode ((int)grid.TreasurePos.x, (int)grid.TreasurePos.y);

			MoveTo mtTreasure = new MoveTo(TreasureFind.DataBase, "TS" , GameObject.Find ("TSLeaf").GetComponent<Image> ());
			MoveTo mtGreenKey = new MoveTo (TreasureFind.DataBase, "GK",GameObject.Find ("GKLeaf").GetComponent<Image> ());
			MoveTo mtBlueKey = new MoveTo (TreasureFind.DataBase, "BK",GameObject.Find ("BKLeaf").GetComponent<Image> ());
			MoveTo mtRedKey = new MoveTo (TreasureFind.DataBase, "RK",GameObject.Find ("RKLeaf").GetComponent<Image> ());
			MoveTo mtGreenDoor = new MoveTo (TreasureFind.DataBase, "GD",GameObject.Find ("GDLeaf").GetComponent<Image> ());
			MoveTo mtBlueDoor = new MoveTo (TreasureFind.DataBase, "BD",GameObject.Find ("BDLeaf").GetComponent<Image> ());
			MoveTo mtRedDoor = new MoveTo ( TreasureFind.DataBase,"RD",GameObject.Find ("RDLeaf").GetComponent<Image> ());

			Sequence greenSeq1 = new Sequence (GameObject.Find ("GKSeq").GetComponent<Image> ());
			Sequence greenSeq2 = new Sequence (GameObject.Find ("GDSeq").GetComponent<Image> ());
			Sequence blueSeq1 = new Sequence (GameObject.Find ("BKSeq").GetComponent<Image> ());
			Sequence blueSeq2 = new Sequence (GameObject.Find ("BDSeq").GetComponent<Image> ());
			Sequence redSeq1 = new Sequence (GameObject.Find ("RKSeq").GetComponent<Image> ());
			Sequence redSeq2 = new Sequence (GameObject.Find ("RDSeq").GetComponent<Image> ());
			Sequence treasureSeq = new Sequence (GameObject.Find ("TSSeq").GetComponent<Image> ());
			Selector rootSel = new Selector (GameObject.Find("Sel").GetComponent<Image>());
			//Selector greenSel = new Selector ("greenSel");
			//Selector redSel = new Selector ("redSel");
			//Selector blueSel = new Selector ("blueSel");
			PriorityRepeater pr1 = new PriorityRepeater (GameObject.Find ("PR").GetComponent<Image> ());
			Happy happy = new Happy (TreasureFind.DataBase, GameObject.Find ("HappyLeaf").GetComponent<Image> ());
			Sad sad = new Sad (TreasureFind.DataBase,GameObject.Find ("SadLeaf").GetComponent<Image> ());

			KeyCheck greenKeyCheck = new KeyCheck (TreasureFind.DataBase, "green", GameObject.Find ("GKCLeaf").GetComponent<Image> ());
			KeyCheck blueKeyCheck = new KeyCheck (TreasureFind.DataBase, "blue", GameObject.Find ("BKCLeaf").GetComponent<Image> ());
			KeyCheck redKeyCheck = new KeyCheck (TreasureFind.DataBase, "red", GameObject.Find ("RKCLeaf").GetComponent<Image> ()) ;
			DoorCheck greenDoorCheck = new DoorCheck (TreasureFind.DataBase, "green",GameObject.Find ("GDCLeaf").GetComponent<Image> ());
			DoorCheck blueDoorCheck = new DoorCheck (TreasureFind.DataBase, "blue",GameObject.Find ("BDCLeaf").GetComponent<Image> ());
			DoorCheck redDoorCheck = new DoorCheck (TreasureFind.DataBase, "red",GameObject.Find ("RDCLeaf").GetComponent<Image> ());
			//greenKeyCheck.Child = mtGree KeyCheck);
			rootSel.Children.Add (pr1);
			rootSel.Children.Add (sad);
			pr1.Children.Add (treasureSeq);
			pr1.Children.Add (greenSeq1);
			pr1.Children.Add (greenSeq2);
			pr1.Children.Add (blueSeq1);
			pr1.Children.Add (blueSeq2);
			pr1.Children.Add (redSeq1);
			pr1.Children.Add (redSeq2);
			//pr1.Children.Add (happy);

			treasureSeq.Children.Add (mtTreasure);
			treasureSeq.Children.Add (happy);

			greenSeq1.Children.Add (greenKeyCheck);
			greenSeq1.Children.Add (mtGreenKey);
			greenSeq2.Children.Add (greenDoorCheck);
			greenSeq2.Children.Add (mtGreenDoor);
			//greenSel.Children.Add (greenSeq1);
			//greenSel.Children.Add (greenSeq2);

			blueSeq1.Children.Add (blueKeyCheck);
			blueSeq1.Children.Add (mtBlueKey);
			blueSeq2.Children.Add (blueDoorCheck);
			blueSeq2.Children.Add (mtBlueDoor);
			//blueSel.Children.Add (blueSeq1);
			//blueSel.Children.Add (blueSeq2);

			redSeq1.Children.Add (redKeyCheck);
			redSeq1.Children.Add (mtRedKey);
			redSeq2.Children.Add (redDoorCheck);
			redSeq2.Children.Add (mtRedDoor);
			//redSel.Children.Add (redSeq1);
			//redSel.Children.Add (redSeq2);

			behaviorT = new BehaviorTree (rootSel, TreasureFind.DataBase);
			
		}

		private void DrawDebugLine(bool debug){
			Steering_Line.transform.rotation = Quaternion.identity;
			Steering_Line.SetPosition (1, Vector2.ClampMagnitude (steering / 8f, 5f));
			Steering_Line.gameObject.SetActive (debug);
		}

		public void ResetChecked(){
			foreach (GridNode nodes in CheckedNode) {
				nodes.SetPathStatus(GridNode.PathStatus.none);
				nodes._text.gameObject.SetActive (false);
			}
		}

		private void DrawPath(bool debug){
			if(CheckedNode != null){
				
				foreach(GridNode nodes in CheckedNode){
						nodes.DrawPath (debug);
						nodes._text.gameObject.SetActive (debug);
				}

			}
		}

		void Pickup ()
		{
			if (grid.PostoRC (_actor.Position) == grid.GreenKeyPos){
				haveGreenKey = true;
				canGetGreenKey = false;
				TreasureFind.DataBase ["haveGreenKey"] = true;
				
				treasureFind._greenkey.transform.position = new Vector2 (28f, 16f);
			}
			if (grid.PostoRC (_actor.Position) == grid.RedKeyPos) {
				haveRedKey = true;
				canGetRedKey = false;
				TreasureFind.DataBase ["haveRedKey"] = true;
				treasureFind._redkey.transform.position = new Vector2 (30f, 16f);
			}
			if (grid.PostoRC (_actor.Position) == grid.BlueKeyPos) {
				haveBlueKey = true;
				TreasureFind.DataBase["haveBlueKey"] = true;
				canGetBlueKey = false;
				treasureFind._bluekey.transform.position = new Vector2 (32f, 16f);
			}
			if (grid.PostoRC (_actor.Position) == grid.TreasurePos) {
				haveTreasure = true;
				treasureFind._treasure.transform.position = new Vector2 (34f, 16f);
			}
			
			
			
		}

		void OpenDoor ()
		{
			if(grid.PostoRC(_actor.Position)==grid.GreenDoorPos && haveGreenKey == true){
				treasureFind.greenDoorOpened = true;
				treasureFind._greendoor.sprite = treasureFind._opendoor;
				TreasureFind.ShutDoor.Remove (treasureFind.greenDoorNode);
				TreasureFind.DataBase ["openedGreenDoor"] = true;
				
			}
			if (grid.PostoRC (_actor.Position) == grid.BlueDoorPos && haveBlueKey == true) {
				treasureFind.blueDoorOpened = true;
				treasureFind._bluedoor.sprite = treasureFind._opendoor;
				TreasureFind.DataBase ["openedBlueDoor"] = true;
				TreasureFind.ShutDoor.Remove (treasureFind.blueDoorNode);
			

		}
			if (grid.PostoRC (_actor.Position) == grid.RedDoorPos && haveRedKey == true) {
				treasureFind.redDoorOpened = true;
				treasureFind._reddoor.sprite = treasureFind._opendoor;
				TreasureFind.ShutDoor.Remove (treasureFind.redDoorNode);
				TreasureFind.DataBase ["openedRedDoor"] = true;
			

		}

		}

		void Awake(){
		}
		// Use this for initialization
		void Start ()
		{
			_actor = GetComponent<IActor> ();
			StartNode = grid.PostoNode (_actor.Position);
			AIBehavior ();
			TreasureFind.DataBase ["haveBlueKey"] = false;
			TreasureFind.DataBase ["haveGreenKey"] = false;
			TreasureFind.DataBase ["haveRedKey"] = false;
			TreasureFind.DataBase ["openedRedDoor"] = false;
			TreasureFind.DataBase ["openedGreenDoor"] = false;
			TreasureFind.DataBase ["openedBlueDoor"] = false;
			
			//bool asd = pathfinder.CheckAvailability (greenDoor,blueDoor);
			//Debug.Log (asd);
		}
		
		// Update is called once per frame
		void Update () {
			behaviorT.Process ();
			//Debug.Log (tarNode);
			//Debug.Log ( behaviorT.Process () );
			//Debug.Log ();
			//			PathFollow (Path);
			Pickup ();
			OpenDoor ();
			DrawDebugLine (true);
			DrawPath (true);
		}
	}
}