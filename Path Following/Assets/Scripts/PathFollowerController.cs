using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox{
	[RequireComponent(typeof (IActor))]
	public class PathFollowerController : MonoBehaviour {
		public Pathfinding pathfinder;
		private IActor _actor;
		public Grid grid;
		private bool arrived = false;
		private List<GridNode> Path;
		private List<GridNode> CheckedNode;
		private GridNode startNode;
		private GridNode goalNode;
		private GridNode tarNode;
		private bool drawPathDebug;
		public LineRenderer Steering_Line;
		private Vector2 steering;
		private bool theOne = false;

		void Start () {
			_actor = GetComponent<IActor > ();	
			Steering_Line.gameObject.SetActive (drawPathDebug);

			NewPath (theOne);
			}

		public GridNode StartNode{ get { return startNode; }set{ startNode = value; } }
		public GridNode GoalNode{ get { return goalNode; }set{ goalNode = value; } }
		public bool DrawPath{ get { return drawPathDebug; } set { drawPathDebug = value; } }
		public bool TheOne{get{return theOne;}set{theOne = value;}}

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

		private void PathFollow (List<GridNode> path)
		{
			
			if (tarNode != path [0]) {
				if (DisToGrid (tarNode) < 1f) {
					tarNode = path [path.IndexOf (tarNode) - 1];
				}
				if (tarNode != goalNode) {
					SeekToGrid (tarNode);
				}
			}


			if (tarNode == path [0]) {
				ArriveAtGrid (path [0]);
				if (DisToGrid (tarNode) < 1f) {
					arrived = true;
				}
		}
		}

//		private void PathFollow2(List<GridNode> path){
//			if (grid.PostoNode (_actor.Position) != path [0]) {
//				GridNode targetNode = path [path.IndexOf (grid.PostoNode (_actor.Position)) - 1];
//				SeekToGrid (targetNode);
//			} else {
//				ArriveAtGrid (path[0]);
//				arrived = true;
//			}
//		}

		private void ClearStartGoal(){
			startNode.DrawNodes ();
			goalNode.DrawNodes ();
		}
			
		private void NewPath (bool theone)
		{
			startNode = grid.PostoNode (_actor.Position);
			goalNode = grid.RandomNode;
			startNode.DrawNodes ();
			goalNode.DrawNodes ();
			if (theone) {
				Path = pathfinder.PathFindTheOne (this).Key;
				CheckedNode = pathfinder.PathFindTheOne (this).Value;
			}
			else{
				Path = pathfinder.PathfindNorm (this).Key;
				CheckedNode = pathfinder.PathfindNorm (this).Value;
			}
			tarNode = Path [Path.Count -1];

		}



		private void DrawDebugLine(bool debug){
			Steering_Line.transform.rotation = Quaternion.identity;
			Steering_Line.SetPosition (1, Vector2.ClampMagnitude( steering/50f,5f));
			Steering_Line.gameObject.SetActive (debug);
		}
		private void PaintPath (bool debug)
		{
			if (CheckedNode != null) {
				if (debug) {
					foreach (GridNode nodes in CheckedNode) {
						nodes.DrawPath (true);
						nodes._text.gameObject.SetActive (true);
					}
					startNode._renderer.sprite = grid.StartSprite;
					goalNode._renderer.sprite = grid.GoalSprite;
				}
				if (!debug) {
					foreach (GridNode nodes in CheckedNode) {
						nodes.DrawPath (false);
						startNode.DrawNodes ();
						goalNode.DrawNodes ();
						nodes._text.gameObject.SetActive (false);
					}
				}

			}
		}

		// Update is called once per frame
		private void Update ()
		{
			
			if (arrived == true) {
				grid.ResetGrid (theOne);
				arrived = false;
				ClearStartGoal ();
				NewPath (theOne);


			}
			PathFollow (Path);


			if (theOne) {
				DrawDebugLine (drawPathDebug);
				PaintPath (drawPathDebug);
			}
		}
	}
}