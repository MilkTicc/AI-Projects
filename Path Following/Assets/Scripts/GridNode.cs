using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

namespace AISandbox {
	public class GridNode : MonoBehaviour {
        public Grid grid;
        public int column;
        public int row;
		public TextMesh _text;
        public SpriteRenderer _renderer;
        private Color _orig_color;
		//private Color _blocked_color;
		private Color _checked_color;
		private Color _wasopen_color;
		//private Color _nopath_color;
		private Color _path_color;
		public GridNode prevNode;
		public bool isStart = false;
		public bool isGoal = false;
		public bool isOnPath = false;
		private int cost=1;
		private int cost_so_far= 0;
		private int heuristic= 0;
		public IList<GridNode> Neighbors = new List<GridNode>();
        [SerializeField]
		public enum NodeStatus
		{
			normal, forest, rock, blocked
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
		public int Cost{ get { return cost; } set { cost = value; } }
        private void Awake() {
            _renderer = GetComponent<SpriteRenderer>();
			SetNodeStatus (GridNode.NodeStatus.normal);
			SetPathStatus (GridNode.PathStatus.none);
			_orig_color = _renderer.color;
			_checked_color = new Color( _orig_color.r * 0.7f, _orig_color.g * 0.6f, _orig_color.b * 0.46f );
			_wasopen_color = new Color ( _orig_color.r * 1f, _orig_color.g * 0.76f, _orig_color.b * 0.32f );
			_path_color = new Color ( _orig_color.r * 1f, _orig_color.g * 1f, _orig_color.b * 0f );


        }

		private void Start(){Neighbors = grid.GetNodeNeighbors (this.row,this.column, true);}
		public void SetText(string texttext){
			_text.text = texttext;
		}

        public IList<GridNode> GetNeighbors( bool include_diagonal = true ) {
            return grid.GetNodeNeighbors( row, column, include_diagonal );
        }

		public int SudoRand(int n){
			int i = ((int)(Mathf.PI * row*row) + (int)Mathf.Sqrt(2)*column)%n;
			return i;
		}

		public void DrawPath(bool debug){
			if (!debug)
				_renderer.color = _orig_color;
			if (debug) {
				switch (pathStatus) {
				case PathStatus.none:
					_renderer.color = _orig_color;
					break;
				case PathStatus._checked:
					_renderer.color = _checked_color;
					break;
				case PathStatus.isOnPath:
					_renderer.color = _path_color;
					break;
				case PathStatus._wasInOpen:
					_renderer.color = _wasopen_color;
					break;
				}
			}
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

			}
		}

		private void Update(){
			if (!PathFollowing.pathFollowStatus)
				DrawNodes ();

			if (Input.GetMouseButtonUp (0) && !PathFollowing.pathFollowStatus)
				Neighbors = grid.GetNodeNeighbors (this.row,this.column, true);
				
		}
						
    }
}