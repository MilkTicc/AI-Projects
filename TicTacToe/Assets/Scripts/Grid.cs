using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;


namespace AISandbox {
	public class Grid : MonoBehaviour {
        public GridNode gridNodePrefab;
        private GridNode[ , ] _nodes;
        private float _node_width;
        private float _node_height;
		private float gridsize=1.15f;
		public static List<GridNode> noPathList = new List<GridNode>();
		public TicTacToe ticTacToe;
		private void Start(){
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

        public void Create(int rows, int columns) {

            _node_width = gridNodePrefab.GetComponent<Renderer>().bounds.size.x* gridsize;
            _node_height = gridNodePrefab.GetComponent<Renderer>().bounds.size.y* gridsize;
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

		public List<GridNode> AllNode{ get { 
				List<GridNode> allnode =new List<GridNode>();
				for (int row = 0; row < _nodes.GetLength(0); ++row) {
					for (int col = 0; col < _nodes.GetLength(1); ++col) {

						allnode.Add (_nodes [row, col]);
					}
				}
				return allnode;
			} }

		public List<GridNode > AvaliableNodes{get{
				List<GridNode> nodes = new List<GridNode> ();
				for (int row = 0; row < _nodes.GetLength (0); ++row) {
					for (int col = 0; col < _nodes.GetLength (1); ++col) {
						if(_nodes[row,col].NodeStatus == GridNode.NodeStat.open)
							nodes.Add (_nodes [row, col]);
					}
				}
				return nodes;

			}}

        public Vector2 size {
            get {
                return new Vector2( _node_width * _nodes.GetLength( 1 ), _node_height * _nodes.GetLength( 0 ) );
            }
        }

        public GridNode GetNode( int row, int col ) {
			if(row>=0&&col >=0 && row<_nodes.GetLength(0)&& col < _nodes.GetLength(1))
				return _nodes[row, col];

			return null;

        }


		public void Restart(){
		}

        public IList<GridNode> GetNodeNeighbors( int row, int col, int neighbortype ) {
            IList<GridNode> neighbors = new List<GridNode>();

            int start_row = Mathf.Max( row - 1, 0 );
            int start_col = Mathf.Max( col - 1, 0 );
            int end_row = Mathf.Min( row + 1, _nodes.GetLength( 0 ) - 1 );
            int end_col = Mathf.Min( col + 1, _nodes.GetLength( 1 ) - 1 );

            for( int row_index = start_row; row_index <= end_row; ++row_index ) {
                for( int col_index = start_col; col_index <= end_col; ++col_index ) {
                    
						if(neighbortype == 0)
                       		neighbors.Add( _nodes[ row_index, col_index ] );
						else if(neighbortype == 1 && _nodes[row_index, col_index].NodeStatus==GridNode.NodeStat.circle)
                       		neighbors.Add (_nodes [row_index, col_index]);
						else if(neighbortype == 2 && _nodes [row_index, col_index].NodeStatus == GridNode.NodeStat.cross)
                       		neighbors.Add (_nodes [row_index, col_index]);
					
                }
            }

			neighbors.Remove (_nodes [row, col]);
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

		public GridNode MouseNode{get{
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
   
