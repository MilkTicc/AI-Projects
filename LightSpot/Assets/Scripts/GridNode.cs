using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour {
	public Grid grid;
	public int col;
	public int row;
	public float size = 1f;
	public SpriteRenderer _renderer;
	public GridNode prevNode;
 	private int cost = 1;
	private int cost_so_far = 0;
	private int heuristic = 0;
	public List<GridNode> Neighbors = new List<GridNode> ();
	public enum NodeStatus
	{
		normal, blocked, wall
	};
	public NodeStatus _NodeStatus {
		get { return nodeStatus; }
	}
	public void SetNodeStatus (NodeStatus s)
	{
		nodeStatus = s;
	}
	private NodeStatus nodeStatus;
	public int CostSoFar { get { return cost_so_far; } set { cost_so_far = value; } }
	public int Heu { get { return heuristic; } set { heuristic = value; } }
	public int Cost { get { return cost; } set { cost = value; } }
	//public Vector2 Pos{ get { return transform.position + new Vector3(size /2,size/2,0 );}}


	void Awake(){
		SetNodeStatus (GridNode.NodeStatus.normal);
		_renderer = GetComponent<SpriteRenderer> ();
	}
	private void Start () { 
		Neighbors = grid.GetNodeNeighbors (this.row, this.col, true);
		//grid.LinkNeighbor (this);
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.tag == "Wall") {
			//Debug.Log ("collision!");
			SetNodeStatus (NodeStatus.wall);
			foreach(GridNode neibor in Neighbors){
				neibor.Neighbors = grid.GetNodeNeighbors (neibor.row, neibor. col, true);
			}

		}
		if (coll.tag == "Obstacles") {
			SetNodeStatus (NodeStatus.blocked);
		}
	}
	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Wall") {
			//Debug.Log ("collision!");
			SetNodeStatus (NodeStatus.wall);
			//foreach(GridNode neibor in Neighbors){
			//	neibor.Neighbors = grid.GetNodeNeighbors (neibor.row, neibor. col, true);
			//}

		}
		if (coll.gameObject.tag == "Obstacle") {
			SetNodeStatus (NodeStatus.blocked);
		}
	}

	void Update () {
		switch(nodeStatus){
		case NodeStatus.normal:
			_renderer.color = Color.yellow;
			break;
		case NodeStatus.wall:
			_renderer.color = Color.black;
			break;
			case NodeStatus.blocked:
			_renderer.color = Color.red;
			break;
		}
	}
}
