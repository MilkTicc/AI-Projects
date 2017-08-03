using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISandbox{
public class MoveState : IStates {
	private readonly TreasureFinderController tFC;
	private IActor _actor;
	public Pathfinding pathFinder;
	public Grid grid;
	public GridNode goalNode;
	public GridNode tarNode;
	private List<GridNode> thePath;
	Vector2 steering = Vector2.zero;
	public string tarName;
		public string Name { get { return "Moving to" + tarName;}}
	// Use this for initialization
	public MoveState(TreasureFinderController tfc){
		tFC = tfc;
		_actor = tFC.GetComponent<IActor> ();
		pathFinder = GameObject.Find("Pathfinding").GetComponent<Pathfinding>();
		grid = GameObject.Find ("Grid").GetComponent<Grid> ();
		goalNode = tfc.GoalNode;
	}

	private float DisToGrid (GridNode node)
	{return (_actor.Position - (Vector2)node.transform.position).magnitude;
	}

	private void SeekToGrid (GridNode tarnode)
	{
		Vector2 pos =tFC.transform.position;
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
			if (posdif.magnitude < 0.1f) {
				ToCalcState ();
			}
	}

	public void SetName(string Name){
			tarName = Name	;
	}

	private void PathFollow (List<GridNode> path)
	{
		if (tarNode != path [0]) {
			SeekToGrid (tarNode);
			if (DisToGrid (tarNode) < 1f) {
				tarNode = path [path.IndexOf (tarNode) - 1];
			}
		}
		if (tarNode == path [0]) {
			ArriveAtGrid (path [0]);
		}
	}

		public void ToCalcState ()
		{
			tFC.currentState = tFC.calcState;
			tFC.calcState.Enter ();
		}

	public void Enter(){
			thePath = pathFinder.PathFindTheOne (tFC).Key;
			tFC.CheckedNode = pathFinder.PathFindTheOne (tFC).Value;
			tarNode = thePath [thePath.Count - 1];
			}

	public void Exit () {
		
	}
	
	// Update is called once per frame
	public void Update () {
			PathFollow (thePath);

	}
}}
