using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox{
	[RequireComponent(typeof (IActor))]
	public class TreasureFinderController : MonoBehaviour {
		public IActor _actor;
		public Pathfinding pathfinder;
		public Grid grid;
		public LineRenderer Steering_Line;
		public TreasureFind treasureFind;
		private List<GridNode> Path;
		public List<GridNode> CheckedNode;
		private Vector2 steering;
		private GridNode startNode;
		private GridNode goalNode;
		private GridNode tarNode;
		public IStates currentState;
		public HappyState happyState;
		public SadState sadState;
		public TestState testState;
		public CalculateState calcState;
		public MoveState moveState;
		[HideInInspector]
		public bool canGetGreenKey, canGetRedKey,canGetBlueKey,canGetGreenDoor,canGetBlueDoor,canGetRedDoor, canGetTreasure;
		[HideInInspector]
		public bool haveGreenKey=false, haveRedKey=false, haveBlueKey=false,haveTreasure=false , cantFindPath;
		public GridNode StartNode{get{return startNode;} set{startNode = value;}}
		public GridNode GoalNode{get{return goalNode;} set{goalNode = value;}}

//		private float DisToGrid (GridNode node)
//		{return (_actor.Position - (Vector2)node.transform.position).magnitude;
//		}
//
//		private void SeekToGrid (GridNode tarnode)
//		{
//			Vector2 pos = _actor.Position;
//			Vector2 tarpos = tarnode.transform.position;
//			Vector2 posdif = tarpos - pos;
//			steering = (posdif.normalized * _actor.MaxSpeed / grid.PostoNode (_actor.Position).Cost - _actor.Velocity) * 50f;
//			_actor.SetAcc (steering.x, steering.y);
//		}
//
//		private void ArriveAtGrid(GridNode tarnode){
//			Vector2 pos = _actor.Position;
//			Vector2 tarpos = tarnode.transform.position;
//			Vector2 posdif = tarpos - pos;
//			float T = posdif.magnitude / 3f;
//			steering = (posdif.normalized * _actor.MaxSpeed* T - _actor.Velocity)*50f;
//			_actor.SetAcc (steering.x, steering.y);
//
//		}
//
//		private void PathFollow (List<GridNode> path)
//		{
//			
//			if (tarNode != path [0]) {
//				if (DisToGrid (tarNode) < 1f) {
//					tarNode = path [path.IndexOf (tarNode) - 1];
//				}
//				if (tarNode != goalNode) {
//					SeekToGrid (tarNode);
//				}
//			}
//			if (tarNode == path [0]) {
//				ArriveAtGrid (path [0]);
//			}
//		}

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

		private void DrawDebugLine(bool debug){
			Steering_Line.transform.rotation = Quaternion.identity;
			Steering_Line.SetPosition (1, Vector2.ClampMagnitude (steering / 8f, 5f));
			Steering_Line.gameObject.SetActive (debug);
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
				treasureFind._greenkey.transform.position = new Vector2 (28f, 16f);
			}
			if (grid.PostoRC (_actor.Position) == grid.RedKeyPos) {
				haveRedKey = true;
				canGetRedKey = false;
				treasureFind._redkey.transform.position = new Vector2 (30f, 16f);
			}
			if (grid.PostoRC (_actor.Position) == grid.BlueKeyPos) {
				haveBlueKey = true;
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
			}
			if (grid.PostoRC (_actor.Position) == grid.BlueDoorPos && haveBlueKey == true) {
				treasureFind.blueDoorOpened = true;
				treasureFind._bluedoor.sprite = treasureFind._opendoor;
				TreasureFind.ShutDoor.Remove (treasureFind.blueDoorNode);
			

		}
			if (grid.PostoRC (_actor.Position) == grid.RedDoorPos && haveRedKey == true) {
				treasureFind.redDoorOpened = true;
				treasureFind._reddoor.sprite = treasureFind._opendoor;
				TreasureFind.ShutDoor.Remove (treasureFind.redDoorNode);
			

		}

		}

		void Awake(){
			calcState = new CalculateState (this);
			moveState = new MoveState (this);
			happyState = new HappyState (this);
			sadState = new SadState (this);
		}
		// Use this for initialization
		void Start () {
			_actor = GetComponent<IActor > ();
			StartNode = grid.PostoNode (_actor.Position);
			currentState = calcState;
			currentState.Enter ();
		}
		
		// Update is called once per frame
		void Update () {
			currentState.Update ();
			//			PathFollow (Path);
			Pickup ();
			OpenDoor ();
			DrawDebugLine (TreasureFind.debugging);
			DrawPath (TreasureFind.debugging);
		}
	}
}