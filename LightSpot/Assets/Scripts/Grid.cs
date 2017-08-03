using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
	public GridNode gridNodePrefab;
	GridNode [,] _nodes;
	Vector2 gridPos;
	float nodeSize = 0.5f;
	int gridRows;
	int gridCols;
	float cameraSize;
	public List<GridNode> allNodes= new List<GridNode>();
	//GameObject [] allNodes;
	public LineRenderer neighborLine;
	

	private GridNode CreateNode (int row, int col)
	{
		GridNode node = Instantiate<GridNode> (gridNodePrefab);
		node.name = string.Format ("Node {0}_{1}", row, col);
		//node.name = string.Format ("Node {0}{1}", (char)('A' + row), col);
		node.grid = this;
		node.row = row;
		node.col = col;
		node.transform.SetParent (transform);
		node.gameObject.SetActive (true);
		//node.transform.localScale = new Vector3 (nodeSize, nodeSize, nodeSize) * 0.178f;
		//node.size = nodeSize;
		//node.transform.position = new Vector2 (row * node.size, col * node.size);
		
		return node;
	}

	public void CreateGrid(int rows, int columns){
		//float nodeSize = gridNodePrefab.size;
		//nodeSize = cameraSize * 2 / (columns +2)
		nodeSize = 0.5f;
		gridRows = rows;
		gridCols = columns;
		//transform.position = Camera.main.ViewportToScreenPoint (Vector3.zero);
		//transform.position = new Vector2 (-7.4f, -3.9f);
		//transform.position = new Vector2 (-cameraSize * 16 /9, -cameraSize) + new Vector2(nodeSize * 1f,nodeSize * 1f);

		_nodes = new GridNode [rows, columns];
		for (int r = 0; r < rows; ++r){
			for (int c = 0; c < columns; c++){
				GridNode node = CreateNode (r, c);
				node.transform.localPosition = new Vector2 (r * nodeSize, c * nodeSize) + new Vector2 (-7.4f, -3.9f);
				//node.transform.localPosition = new Vector2 (r * nodeSize, c * nodeSize);
				_nodes [r, c] = node;
				allNodes.Add (node);
			}
		}
	}

	//List<GridNode> AllNode {
	//	get {
	//		List<GridNode> allnode = new List<GridNode> ();
	//		for (int row = 0; row < 20; ++row) {
	//			for (int col = 0; col < 32; ++col) {

	//				allnode.Add (_nodes [row, col]);
	//			}
	//		}
	//		return allnode;
	//	}
	//}

	public GridNode GetNode (int row, int col)
	{
		return _nodes [row, col];
	}

	//public GridNode RandomNode {
	//	get {
	//		GridNode radomnodes = _nodes [Random.Range (0, gridRows), Random.Range (0, gridCols)];
	//		while (radomnodes._NodeStatus != GridNode.NodeStatus.normal) {
	//			radomnodes = _nodes [Random.Range (0, gridRows), Random.Range (0, gridCols)];
	//		}
	//		return radomnodes;
	//	}
	//}
	
	public List<GridNode> GetNodeNeighbors (int row, int col, bool include_diagonal = true)
	{
		List<GridNode> neighbors = new List<GridNode> ();

		int start_row = Mathf.Max (row - 1, 0);
		int start_col = Mathf.Max (col - 1, 0);
		int end_row = Mathf.Min (row + 1, _nodes.GetLength (0) - 1);
		int end_col = Mathf.Min (col + 1, _nodes.GetLength (1) - 1);

		for (int row_index = start_row; row_index <= end_row; ++row_index) {
			for (int col_index = start_col; col_index <= end_col; ++col_index) {
				if (row_index == row || col_index == col) {
					neighbors.Add (_nodes [row_index, col_index]);
				}
			}
		}
		if (include_diagonal) {
			if (row != _nodes.GetLength (0) - 1 && col != _nodes.GetLength (1) - 1 && _nodes [row + 1, col]._NodeStatus == GridNode.NodeStatus.normal && _nodes [row, col + 1]._NodeStatus == GridNode.NodeStatus.normal)
				neighbors.Add (_nodes [row + 1, col + 1]);
			if (row != 0 && col != _nodes.GetLength (1) - 1 && _nodes [row - 1, col]._NodeStatus == GridNode.NodeStatus.normal && _nodes [row, col + 1]._NodeStatus == GridNode.NodeStatus.normal)
				neighbors.Add (_nodes [row - 1, col + 1]);
			if (row != _nodes.GetLength (0) - 1 && col != 0 && _nodes [row + 1, col]._NodeStatus == GridNode.NodeStatus.normal && _nodes [row, col - 1]._NodeStatus == GridNode.NodeStatus.normal)
				neighbors.Add (_nodes [row + 1, col - 1]);
			if (row != 0 && col != 0 && _nodes [row - 1, col]._NodeStatus == GridNode.NodeStatus.normal && _nodes [row, col - 1]._NodeStatus == GridNode.NodeStatus.normal)
				neighbors.Add (_nodes [row - 1, col - 1]);
		}

		return neighbors;
	}

	public GridNode PostoNode(Vector3 pos){
		//Vector2 worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		//(pos-_nodes[1,1].transform.position).x / nodeSize
		int row = Mathf.FloorToInt ((pos.x+nodeSize/2- _nodes[0,0].transform.position.x) / nodeSize);
		int col = Mathf.FloorToInt ((pos.y +nodeSize/2 - _nodes [0, 0].transform.position.y)/ nodeSize);
		//Debug.Log (row+"  "+ col);
		if( row >=0 && row < _nodes.GetLength(0) && col >=0 && col < _nodes.GetLength(1)){
			return _nodes [row, col];
		}
		Debug.Log ("null");
		return null;
	}


	void Awake(){
		cameraSize = Camera.main.orthographicSize;
	}
	public void LinkNeighbor (GridNode node) {
		//GameObject[] temp = GameObject.FindGameObjectsWithTag ("GridNode");
		//foreach(GameObject gameobject in temp ){
		//	allNodes.Add (gameobject.GetComponent<GridNode> ());
		//}
		//for (int i = 0; i < allNodes.Count; i++) {
		//	Debug.Log (i);
			for (int j = 0; j <node.Neighbors.Count; j++) {
				//Debug.Log (i + " " + j);
				LineRenderer newLine = Instantiate<LineRenderer> (neighborLine);
				newLine.SetPosition (0, node.transform.position);
				newLine.SetPosition (1, node.Neighbors [j].transform.position);
				newLine.transform.parent = neighborLine.transform;
			}
		//}
	}

	public void ShowNodes(List<GridNode> list){
		foreach(GridNode node in list){
			node._renderer.color = Color.green;
		}
	}

	public void ResetNodes(){
		foreach (GridNode node in allNodes) {
			if (node._NodeStatus == GridNode.NodeStatus.normal)
				node._renderer.color = Color.yellow;
		}
	}

	public void ResetGrid(){
		foreach(GridNode node in allNodes){
			node.CostSoFar = 0;
			node.Heu = 0;
		}
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (_nodes[0,0].Pos);
		//Debug.Log (PostoNode(Input.mousePosition));
		
	}
}
