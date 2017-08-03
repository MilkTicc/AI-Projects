using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatrolState : IStates  {
	private readonly Guard self;
	private IActor _actor;
	public Pathfinding pathFinder;
	public Grid grid;
	public GridNode goalNode;
	public GridNode tarNode;
	private List<GridNode> thePath;
	List<GridNode> patrolNodes;
	int patrolNodeIndex=0;
	float waitTime;
	bool startWaiting;
	bool finishedWaiting;
	

	Vector2 steering = Vector2.zero;
	public string tarName;
		public string Name { get { return "Moving to" + tarName;}}
	// Use this for initialization
	public PatrolState(Guard guard, List<GridNode> list){
		self = guard;
		patrolNodes = guard.patrolNodes;
		_actor = self.GetComponent<IActor> ();
		pathFinder = GameObject.Find("GameManager").GetComponent<Pathfinding>();
		grid = GameObject.Find ("Grid").GetComponent<Grid> ();
		goalNode = patrolNodes[patrolNodeIndex];
	}

	private float DisToGrid (GridNode node)
	{return (self.transform.position - node.transform.position).magnitude;
	}

	private void SeekToGrid (GridNode tarnode)
	{
		Vector2 pos =self.transform.position;
		Vector2 tarpos = tarnode.transform.position;
		Vector2 posdif = tarpos - pos;
		steering = (posdif.normalized * _actor.MaxSpeed / grid.PostoNode (pos).Cost - _actor.Velocity) * 5f;
		_actor.SetAcc (steering.x, steering.y);
	}

	private void ArriveAtGrid(GridNode tarnode){
		Vector2 pos = self.transform.position;
		Vector2 tarpos = tarnode.transform.position;
		Vector2 posdif = tarpos - pos;
		float T = posdif.magnitude / 1.3f;
		steering = (posdif.normalized * _actor.MaxSpeed* T - _actor.Velocity)*5f;
		_actor.SetAcc (steering.x, steering.y);
			if (posdif.magnitude < 0.2f) {
			//ToCalcState ();
			waitTime += Time.deltaTime;
			//_actor.SetAcc (steering.x / 100, steering.y / 100);
			if(waitTime>=1.5f)
			{NextPatrolNode ();
				Enter ();
				waitTime = 0;
			}

			}
	}


	void NextPatrolNode(){
		if (patrolNodeIndex < patrolNodes.Count - 1)
			patrolNodeIndex += 1;
		else
			patrolNodeIndex = 0;
		goalNode = patrolNodes [patrolNodeIndex];
	}

	public void SetName(string Name){
			tarName = Name	;
	}

	private void PathFollow (List<GridNode> path)
	{
		if (tarNode != path [0]) {
			SeekToGrid (tarNode);
			//Debug.Log (DisToGrid(tarNode));
			if (DisToGrid (tarNode) < 0.1f) {
				tarNode = path [path.IndexOf (tarNode) - 1];
			}
		}
		if (tarNode == path [0]) {
			ArriveAtGrid (path [0]);
		}
	}


	public void Enter(){
		thePath = pathFinder.FindPath (self, goalNode);
		if (thePath.Count >= 2)
			tarNode = thePath [thePath.Count - 2];
		else
			tarNode = thePath [0];			}

	public void Exit () {
		
	}
	
	public void Update () {
			PathFollow (thePath);

	}
}
