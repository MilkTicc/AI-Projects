using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;


namespace AISandbox {
	public class Grid : MonoBehaviour {
        public GridNode gridNodePrefab;
        private GridNode[ , ] _nodes;
		public Sprite GoalSprite;
		public Sprite StartSprite;
		public Sprite NormalSprite;
		public Sprite JailSprite;
		public Sprite BaseSprite;
		public UI ui;
		public int GRIDLENGTH = 31;
		public int GRIDHEIGHT = 20;
        private float _node_width;
        private float _node_height;
        private bool _draw_blocked;
//		private GridNode StartNode;
//		private GridNode GoalNode;
		public bool newpath = false;
		//public bool nopath = false;
		public enum DrawStatus{normal,forest,rock,blocked}
		public Sprite[] ForestSprite;
		public Sprite[] RockSprite;
		public Sprite WallSprite;
		private DrawStatus drawStatus;
		private float gridsize=1.491465f;

		//public string map1 = "map1";
		//string map1string;

//		public GridNode RedKey;
//		public GridNode BlueKey;
//		public GridNode GreenDoor;
//		public GridNode RedDoor;
//		public GridNode BlueDoor;
		public List<GridNode> BlockedNodes;
		public List<GridNode> RockNodes;
		public List<GridNode> ForestNodes;
		public DrawStatus _DrawStatus{
			get{return drawStatus ;}
		}
		public void SetDrawStatus(DrawStatus s){
			drawStatus = s;
		}

		private void Awake(){
			Create (GRIDHEIGHT, GRIDLENGTH);
			Vector2 gridSize = size;
			Vector2 gridPos = new Vector2 (gridSize.x * -0.5f, gridSize.y * 0.5f);
			transform.position = gridPos;
		}

		private void Start(){
			SetDrawStatus (DrawStatus.blocked);
			//TextAsset txtAsset = Resources.Load ("map1") as TextAsset;
			//map1string = txtAsset.text;
		}

        private GridNode CreateNode( int row, int col ) {
            GridNode node = Instantiate<GridNode>( gridNodePrefab );
            node.name = string.Format( "Node {0}{1}", (char)('A'+row), col );
            node.grid = this;
            node.row = row;
			node.column = col;
			node.transform.SetParent( transform);
            node.gameObject.SetActive( true );
			node.transform.localScale = new Vector3 (gridsize,gridsize,gridsize);
            return node;
        }
		Vector2 RandomPos(){
			return new Vector2 (Random.Range (1, GRIDHEIGHT - 1), Random.Range (1,GRIDLENGTH - 1));
		}
//		public GridNode getGoalNode{ get { return GoalNode;} }
//		public GridNode getStartNode{ get { return StartNode;} }
		public float NodeWidth{get{return _node_width;}}

        public void Create(int rows, int columns) {

            _node_width = gridNodePrefab.GetComponent<Renderer>().bounds.size.x* 1.50f;
            _node_height = gridNodePrefab.GetComponent<Renderer>().bounds.size.y* 1.50f;
            Vector2 node_position = new Vector2( _node_width * 0.5f-4f, _node_height * -0.5f );
            _nodes = new GridNode[ rows, columns ];
            for( int row = 0; row < rows; ++row ) {
                for( int col = 0; col < columns; ++col ) {
                    GridNode node = CreateNode( row, col );
                    node.transform.localPosition = node_position;
                    _nodes[ row, col ] = node;

                    node_position.x += _node_width;
                }
                node_position.x = _node_width * 0.5f-4f;
                node_position.y -= _node_height;
            }
        }

		void DeleteNode(int row, int col){
			GameObject delete = GameObject.Find (string.Format( "Node {0}{1}", (char)('A'+row-1), col-1 ));
			delete.gameObject.SetActive(false);
			
		}

		List<GridNode> AllNode{ get { 
				List<GridNode> allnode =new List<GridNode>();
				for (int row = 0; row < GRIDHEIGHT; ++row) {
					for (int col = 0; col < GRIDLENGTH; ++col) {

						allnode.Add (_nodes [row, col]);
					}
				}
				return allnode;
			} }


        public Vector2 size {
            get {
                return new Vector2( _node_width * _nodes.GetLength( 1 ), _node_height * _nodes.GetLength( 0 ) );
            }
        }

        public GridNode GetNode( int row, int col ) {
            return _nodes[row, col];
        }
        public GridNode GetNode(Vector2 pos){
			return _nodes [(int)pos.x, (int)pos.y];
        }

		public GridNode RandomNode{get{
				GridNode radomnodes = _nodes [Random.Range (1, GRIDHEIGHT -2 ), Random.Range (1, GRIDLENGTH -2 )];
			while( radomnodes._NodeStatus == GridNode.NodeStatus.blocked){
					radomnodes = _nodes [Random.Range (1, GRIDHEIGHT-2), Random.Range (1, GRIDLENGTH -2)];
			}
				return radomnodes;
			}}

		public void ResetGrid ()
		{
			
			foreach (GridNode nodes in AllNode) {
				
				nodes.CostSoFar = 0;
				nodes.Heu = 0;
			
				//nodes.SetText ("");
				nodes.SetPathStatus (GridNode.PathStatus.none);
				//nodes.DrawPath (TreasureFind.treasureFind);
			}
		}

		public void DecolorGrid(){
			foreach (GridNode nodes in AllNode){
				nodes._renderer.color = nodes._orig_color;
			}
		}

        public IList<GridNode> GetNodeNeighbors( int row, int col, bool include_diagonal = true ) {
            IList<GridNode> neighbors = new List<GridNode>();

            int start_row = Mathf.Max( row - 1, 0 );
            int start_col = Mathf.Max( col - 1, 0 );
            int end_row = Mathf.Min( row + 1, _nodes.GetLength( 0 ) - 1 );
            int end_col = Mathf.Min( col + 1, _nodes.GetLength( 1 ) - 1 );

            for( int row_index = start_row; row_index <= end_row; ++row_index ) {
                for( int col_index = start_col; col_index <= end_col; ++col_index ) {
                    if( row_index == row || col_index == col ) {
                        neighbors.Add( _nodes[ row_index, col_index ] );
                    }
                }
            }
            if(include_diagonal){
				if (row!=_nodes.GetLength( 0 ) - 1&& col!= _nodes.GetLength( 1 ) - 1 &&_nodes[row+1,col]._NodeStatus != GridNode.NodeStatus.blocked && _nodes[row,col+1]._NodeStatus != GridNode.NodeStatus.blocked)
					neighbors.Add (_nodes [row + 1, col + 1]);
				if (row!= 0&&col!= _nodes.GetLength( 1 ) - 1&&_nodes[row-1,col]._NodeStatus != GridNode.NodeStatus.blocked && _nodes[row,col+1]._NodeStatus != GridNode.NodeStatus.blocked)
					neighbors.Add (_nodes [row - 1, col + 1]);
				if (row!=_nodes.GetLength( 0 ) - 1&&col!=0&&_nodes[row+1,col]._NodeStatus != GridNode.NodeStatus.blocked && _nodes[row,col-1]._NodeStatus != GridNode.NodeStatus.blocked)
					neighbors.Add (_nodes [row + 1, col - 1]);
				if (row!= 0&&col!=0&&_nodes[row-1,col]._NodeStatus != GridNode.NodeStatus.blocked && _nodes[row,col-1]._NodeStatus != GridNode.NodeStatus.blocked)
					neighbors.Add (_nodes [row - 1, col - 1]);
				}
			
            return neighbors;
        }

		public List<GridNode> GetSurroundNodesByLevel(GridNode node, int x){
			List<GridNode> neighbors = new List<GridNode> ();
			int start_row = Mathf.Max (node.row - x, 0);
			int start_col = Mathf.Max (node.column - x, 0);
			int end_row = Mathf.Min (node.row + x, _nodes.GetLength (0) - 1);
			int end_col = Mathf.Min (node.column + x, _nodes.GetLength (1) - 1);

			for (int row_index = start_row; row_index <= end_row; ++row_index) {
				for (int col_index = start_col; col_index <= end_col; ++col_index) {
					if (System.Math.Abs(row_index - node.row) + System.Math.Abs(col_index - node.column) > x) {
						continue;
					}
					neighbors.Add (_nodes [row_index, col_index]);
				}
			}

			return neighbors;

		}

		public Vector2 RCtoPosition(int row, int col){
			
			return _nodes[row,col].transform.position;
		}

		public Vector2 RCtoPosition(Vector2 pos){
			return _nodes [(int)pos.x, (int)pos.y].transform.position;
			}

		public GridNode PostoNode(Vector2 pos){
			Vector2 local_pos= transform.InverseTransformPoint (pos);
			int column = Mathf.FloorToInt (local_pos.x / _node_width) + 2;
			int row = Mathf.FloorToInt (-local_pos.y / _node_height);
			if (row >= 0 && row < _nodes.GetLength (0)
				&& column >= 0 && column < _nodes.GetLength (1)) {
				return _nodes [row, column];

			}
			return null;
		}


		public int heuristic (GridNode note1, GridNode note2)
		{
			return Mathf.Abs (note1.row - note2.row) + Mathf.Abs (note1.column - note2.column);
		}

		public Vector2 PostoRC (Vector2 pos)
		{
			Vector2 local_pos = transform.InverseTransformPoint (pos);
			int column = Mathf.FloorToInt (local_pos.x / _node_width) + 2;
			int row = Mathf.FloorToInt (-local_pos.y / _node_height);
			return new Vector2 (row, column);
		}

		private Vector2 MouseRC{ get { 
				Vector3 world_pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector3 local_pos = transform.InverseTransformPoint (world_pos);
				// This trick makes a lot of assumptions that the nodes haven't been modified since initialization.
				int column = Mathf.FloorToInt (local_pos.x / _node_width)+2;
				int row = Mathf.FloorToInt (-local_pos.y / _node_height);
				return new Vector2 (row, column);
			
			} }
		
		void ButtonSwitch(){
			switch (drawStatus) {
			case DrawStatus.normal:
				ui.Reset ();
				ui.Road.color = ui.selected_color;
				ui.Road.transform.localScale = new Vector3 (1.8f, 1.8f, 1.8f);
				break;
			case DrawStatus.forest:
				ui.Reset ();
				ui.Forest.color =ui.selected_color;
				ui.Forest.transform.localScale = new Vector3 (1.8f, 1.8f, 1.8f);
				break;
			case DrawStatus.rock:
				ui.Reset ();
				ui.Rock.color = ui.selected_color;
				ui.Rock.transform.localScale = new Vector3 (1.8f, 1.8f, 1.8f);
				break;
			case DrawStatus.blocked:
				ui.Reset ();
				ui.Wall.color = ui.selected_color;
				ui.Wall.transform.localScale = new Vector3 (1.8f, 1.8f, 1.8f);
				break;
			
			}

		}

		GridNode MouseNode{get{
				int row = Mathf.RoundToInt( MouseRC.x);
				int col = Mathf.RoundToInt(MouseRC.y);
				if (MouseRC.x >= 0 && MouseRC.x < _nodes.GetLength (0)
					&& MouseRC.y >= 0 && MouseRC.y < _nodes.GetLength (1)) {
					GridNode node = _nodes [row,col];
					return node;
				} 

					return null;

			}
		}

        void Update ()
		{
			//if (!TreasureFind.treasureFind)
				ButtonSwitch ();

//			if (Input.GetKeyDown (KeyCode.R)) {
//				foreach (GridNode nei in _nodes [0, 0].Neighbors)
//					print (nei.name);
//			}
			//Select Draw Mode
			if (Input.GetKeyDown (KeyCode.Alpha1)) 
				SetDrawStatus (DrawStatus.normal);
			if (Input.GetKeyDown (KeyCode.Alpha2))
				SetDrawStatus (DrawStatus.forest);
			if (Input.GetKeyDown (KeyCode.Alpha3))
				SetDrawStatus (DrawStatus.rock);
			if (Input.GetKeyDown (KeyCode.Alpha4))
				SetDrawStatus (DrawStatus.blocked);
			
			
			//Draw Blocks
			if ( Input.GetMouseButton( 0 ) && MouseNode) {

				if( Input.GetMouseButton( 0 ) && drawStatus == Grid.DrawStatus.blocked && MouseNode.isGoal == false && MouseNode.isStart==false ) {

					MouseNode.SetNodeStatus(GridNode.NodeStatus.blocked);
					newpath = true;
                    }

				if( Input.GetMouseButton( 0 ) && drawStatus == Grid.DrawStatus.forest && MouseNode.isGoal == false && MouseNode.isStart==false ) {

					MouseNode.SetNodeStatus(GridNode.NodeStatus.forest);
					newpath = true;
				}

				if( Input.GetMouseButton( 0 ) && drawStatus == Grid.DrawStatus.rock && MouseNode.isGoal == false && MouseNode.isStart==false ) {

					MouseNode.SetNodeStatus(GridNode.NodeStatus.rock);
					newpath = true;
				}

				if( Input.GetMouseButton( 0 ) && drawStatus == Grid.DrawStatus.normal && MouseNode.isGoal == false && MouseNode.isStart==false ) {

					MouseNode.SetNodeStatus(GridNode.NodeStatus.normal);
					newpath = true;
                }

            }

		}
        }
    }
   
