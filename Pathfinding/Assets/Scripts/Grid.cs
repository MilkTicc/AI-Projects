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
        private float _node_width;
        private float _node_height;
        private bool _draw_blocked;
		private GridNode StartNode;
		private GridNode GoalNode;
		public bool newpath = false;
		public bool nopath = false;
		private float gridsize=1.491455f;
		public static List<GridNode> noPathList = new List<GridNode>();
		private string[] noPathCord = new string[152] {"D1","D5","D7","D8","D9","D10","D13","D14","D15","D16","D18","D19","D20","D21","D23","D24","D25","D27","D30",
			"E1","E2","E5","E7","E10","E13","E16","E18","E21","E24","E27","E30","F1","F2","F5","F7","F10","F13","F16","F18","F21","F24","F27","F30","G1","G2","G5",
			"G7","G10","G13","G16","G18","G21","G24","G27","G30","H1","H3","H5","H7","H10","H13","H16","H18","H21","H24","H27","H30","I1","I3","I5","I7","I10","I13",
			"I16","I18","I21","I24","I27","I30","J1","J3","J5","J7","J10","J13","J14","J15","J16","J18","J19","J20","J21","J24","J27","J28","J29","J30","K1","K4",
			"K5","K7","K10","K13","K18","K21","K24","K27","K30","L1","L4","L5","L7","L10","L13","L18","L21","L24","L27","L30","M1","M4","M5","M7","M10","M13","M18",
			"M21","M24","M27","M30","N1","N5","N7","N10","N13","N18","N21","N24","N27","N30","O1","O5","O7","O8","O9","O10","O13","O18","O21","O24","O27","O30"};

		private void Start(){
			foreach (string cord in noPathCord) {
				noPathList.Add ( GameObject.Find ("Node " + cord).GetComponent<GridNode >());
			}
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

		public GridNode getGoalNode{ get { return GoalNode;} }
		public GridNode getStartNode{ get { return StartNode;} }

        public void Create(int rows, int columns) {

            _node_width = gridNodePrefab.GetComponent<Renderer>().bounds.size.x* 1.50f;
            _node_height = gridNodePrefab.GetComponent<Renderer>().bounds.size.y* 1.50f;
            Vector2 node_position = new Vector2( _node_width * 0.5f, _node_height * -0.5f );
            _nodes = new GridNode[ rows, columns ];
            for( int row = 0; row < rows; ++row ) {
                for( int col = 0; col < columns; ++col ) {
                    GridNode node = CreateNode( row, col );
                    node.transform.localPosition = node_position;
                    _nodes[ row, col ] = node;

                    node_position.x += _node_width;
                }
                node_position.x = _node_width * 0.5f;
                node_position.y -= _node_height;
            }
        }

		private void DeleteNode(int row, int col){
			GameObject delete = GameObject.Find (string.Format( "Node {0}{1}", (char)('A'+row-1), col-1 ));
			delete.gameObject.SetActive(false);
			
		}

		private List<GridNode> AllNode{ get { 
				List<GridNode> allnode =new List<GridNode>();
				for (int row = 0; row < 18; ++row) {
					for (int col = 0; col < 32; ++col) {

						allnode.Add (_nodes [row, col]);
					}
				}
				return allnode;
			} }

		private GridNode SetGoal(int row, int col){
			GoalNode =  _nodes[row,col];
			GoalNode.isStart = false;
			GoalNode._renderer.sprite = GoalSprite;
			GoalNode.isGoal = true;
			newpath = true;
			return GoalNode;
		}

		private GridNode SetGoal(GridNode node){
			node.isStart = false;
			GoalNode =  node;
			GoalNode._renderer.sprite = GoalSprite;
			GoalNode.isGoal = true;
			newpath = true;
			return GoalNode;		
		}

		public GridNode SetStart(int row, int col){
			StartNode = _nodes[row,col];
			StartNode._renderer.sprite = StartSprite;
			StartNode.isGoal = false;
			StartNode.isStart = true;
			newpath = true;
			return StartNode;
		}

		public GridNode SetStart(GridNode node){
			StartNode = node;
			StartNode.isGoal = false;
			StartNode._renderer.sprite = StartSprite;
			StartNode.isStart = true;
			newpath = true;
			return StartNode;
		}

		private void ClearGoal(){
			if (GoalNode != null) {
				GoalNode.isGoal = false;
				GoalNode._renderer.sprite = NormalSprite;
				GoalNode = null;
			}
		}

		private void ClearStart(){
			if (StartNode != null) {
				StartNode.isStart = false;
				StartNode._renderer.sprite = NormalSprite;
				StartNode = null;
			}
		
		}

        public Vector2 size {
            get {
                return new Vector2( _node_width * _nodes.GetLength( 1 ), _node_height * _nodes.GetLength( 0 ) );
            }
        }

        public GridNode GetNode( int row, int col ) {
            return _nodes[row, col];
        }

		public void RefindPath(){
			//print ("PathFindRestart");
			nopath = false; 
			foreach (GridNode nodes in AllNode) {
					nodes.SetText ("");
					nodes.Cost = 0;
					nodes.Heu = 0;
					nodes.ischecked = false;
					nodes.WasInOpen = false;
					nodes.isOnPath = false;
			}
		}

		public void Restart(){
			ClearGoal ();
			ClearStart ();
			RefindPath ();
			foreach (GridNode nodes in AllNode) {
				nodes.blocked = false;
			}
		}

        public IList<GridNode> GetNodeNeighbors( int row, int col, bool include_diagonal = false ) {
            IList<GridNode> neighbors = new List<GridNode>();

            int start_row = Mathf.Max( row - 1, 0 );
            int start_col = Mathf.Max( col - 1, 0 );
            int end_row = Mathf.Min( row + 1, _nodes.GetLength( 0 ) - 1 );
            int end_col = Mathf.Min( col + 1, _nodes.GetLength( 1 ) - 1 );

            for( int row_index = start_row; row_index <= end_row; ++row_index ) {
                for( int col_index = start_col; col_index <= end_col; ++col_index ) {
                    if( include_diagonal || row_index == row || col_index == col ) {
                        neighbors.Add( _nodes[ row_index, col_index ] );
                    }
                }
            }
			//neighbors.Remove (_nodes [row, col]);
            return neighbors;
        }

		
		private Vector2 MouseRC{ get { 
				Vector3 world_pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector3 local_pos = transform.InverseTransformPoint (world_pos);
				// This trick makes a lot of assumptions that the nodes haven't been modified since initialization.
				int column = Mathf.FloorToInt (local_pos.x / _node_width);
				int row = Mathf.FloorToInt (-local_pos.y / _node_height);
				return new Vector2 (row, column);
			
			} }

//		public void 

		private GridNode MouseNode{get{
				int row = Mathf.RoundToInt( MouseRC.x);
				int col = Mathf.RoundToInt(MouseRC.y);
				if (MouseRC.x >= 0 && MouseRC.x < _nodes.GetLength (0)
					&& MouseRC.y >= 0 && MouseRC.y < _nodes.GetLength (1)) {
					GridNode node = _nodes [row,col];
					return node;
				} else {
					return null;
				}
			}
		}
        private void Update() {
			//Draw if no path

			//Reset Goal Node
			if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown( 0 ) && MouseNode ) {
				if (GoalNode != null) {
					ClearGoal ();
				}
				MouseNode.blocked = false;
				SetGoal (MouseNode);

			}

			//Reset Start Node
			if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown( 0 ) && MouseNode ) {
				if (StartNode != null) {
					ClearStart ();
				}
				MouseNode.blocked = false;
				SetStart (MouseNode);

			}

			//Debug
			if (Input.GetKey(KeyCode.Tab) && Input.GetMouseButtonDown( 0 ) && MouseNode ) {
				print ("Hue: " + MouseNode.Heu + ", Cost: " + MouseNode.Cost);
			}

//			if (Input.GetKey (KeyCode.Tab)) {
////				for( int row = 0; row < 18; ++row ) {
////					for( int col = 0; col < 32; ++col ) {
////						if (_nodes [row,col].blocked) {
////							print (_nodes [row,col].gameObject.name);
////						}
////					}
////
////				}
//				foreach(GridNode node in noPathList){
//					print (node.name);
//				}

//			}
			//Draw Obstacles
			if ( Input.GetMouseButton( 0 ) && MouseNode && !Input.GetKey(KeyCode.Tab)) {

				if( Input.GetMouseButtonDown( 0 )&& MouseNode.isGoal == false && MouseNode.isStart==false ) {
					_draw_blocked = !MouseNode.blocked;
					newpath = true;
                    }


				if( MouseNode.blocked != _draw_blocked  && MouseNode.isGoal == false && MouseNode.isStart==false ) {
					MouseNode.blocked = _draw_blocked;
					newpath = true;
                    }
                }
//			if(Input.GetMouseButton( 0 ) && ((MouseNode.isGoal == true) ||(MouseNode.isStart ==true )) ){
//				GridNode tempnode = MouseNode;
//				tempnode.transform.localScale = new Vector3(1.6f,1.6f,1.6f);
//				tempnode.transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);

//			}
//			if (Input.GetMouseButtonUp(0)) {
//				MouseNode.transform.localScale = new Vector3(gridsize,gridsize,gridsize);
			}
            }
        }
   
