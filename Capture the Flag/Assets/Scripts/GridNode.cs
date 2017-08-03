using UnityEngine;
using System.Collections.Generic;

namespace AISandbox {
	public class GridNode : MonoBehaviour {
        public Grid grid;
        public int column;
        public int row;
		public TextMesh _text;
        public SpriteRenderer _renderer;
        public Color _orig_color;
		public GridNode prevNode;
		public bool isStart = false;
		public bool isGoal = false;
		public bool isOnPath = false;
		int cost=1;
		int cyanInfluence = 0;
		int mangInfluence = 0;
		int cost_so_far= 0;
		int heuristic= 0;
		public IList<GridNode> Neighbors = new List<GridNode>();
        [SerializeField]
		public enum NodeStatus
		{
			normal, forest, rock, blocked, jail, flagbase
		};
		private NodeStatus nodeStatus;
		public enum PathStatus
		{
			none, _checked, _wasInOpen, isOnPath
		}
		private PathStatus pathStatus;

		public NodeStatus _NodeStatus{
			get{return nodeStatus ;}
		}
		public void SetNodeStatus(NodeStatus s){
			nodeStatus = s;
		}

		public PathStatus _PathStatus{ get { return pathStatus; } }
		public void SetPathStatus(PathStatus s){
			pathStatus = s;
		}

		public int CostSoFar{get{return cost_so_far;} set{ cost_so_far = value;}}
		public int Heu{get{return heuristic;} set{ heuristic = value;}}
		public int Cost { get { return cost; } set { cost = value; } }
		public int CyanInfluence { get { return cyanInfluence; } set { cyanInfluence = value; } }
		public int MangInfluence { get { return mangInfluence; } set { mangInfluence = value; } }
        private void Awake() {
            _renderer = GetComponent<SpriteRenderer>();
			SetNodeStatus (GridNode.NodeStatus.normal);
			SetPathStatus (GridNode.PathStatus.none);
			_orig_color = _renderer.color;
        }

		private void Start(){Neighbors = grid.GetNodeNeighbors (this.row,this.column, true);}
		//public void SetText(string texttext){
		//	_text.text = texttext;
		//}

        public IList<GridNode> GetNeighbors( bool include_diagonal = true ) {
            return grid.GetNodeNeighbors( row, column, include_diagonal );
        }

		public int SudoRand(int n){
			int i = ((int)(Mathf.PI * row*row) + (int)Mathf.Sqrt(2)*column)%n;
			return i;
		}

		public void DrawNodes(){
			switch (nodeStatus) {
			case NodeStatus.normal:
				cost = 1;
				if (isGoal)
					_renderer.sprite = grid.GoalSprite;
				else if (isStart)
					_renderer.sprite = grid.StartSprite;
				else
					_renderer.sprite = grid.NormalSprite;
				break;
			case NodeStatus.forest:
				cost = 2;
				_renderer.sprite = grid.ForestSprite[SudoRand (3)];
				break;
			case NodeStatus.rock:
				cost = 3;
				_renderer.sprite = grid.RockSprite[SudoRand (2)];
				break;
			case NodeStatus.blocked:
				_renderer.sprite = grid.WallSprite;
				break;
			case NodeStatus.jail:
				_renderer.sprite = grid.JailSprite;
				break;
			case NodeStatus.flagbase:
				_renderer.sprite = grid.BaseSprite;
				break;

			}
		}

		private void Update(){
			//if (!TreasureFind.treasureFind)
				DrawNodes ();
	
		}
						
    }
}