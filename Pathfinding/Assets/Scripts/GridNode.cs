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
		private Color _blocked_color;
		private Color _checked_color;
		private Color _wasopen_color;
		private Color _nopath_color;
		private Color _path_color;
		public GridNode prevNode;
		public bool isStart = false;
		public bool isGoal = false;
		public bool isOnPath = false;
		private int cost= 0;
		private int heuristic= 0;
        [SerializeField]
        private bool _blocked = false;
		private bool _checked = false;
		private bool _wasInOpen = false;
        public bool blocked {
            get {
                return _blocked;
            }
            set {
                _blocked = value;
               // _renderer.color = _blocked ? _blocked_color : _renderer.color;
            }
        }

		public bool ischecked {
			get {
				return _checked;
			}
			set {
				_checked = value;
				//_renderer.color = _checked ? _checked_color : _orig_color;
			}
		}

		public bool WasInOpen {
			get {
				return _wasInOpen;
			}
			set {
				_wasInOpen = value;

			}
		}

		public int Cost{get{return cost;} set{ cost = value;}}
		public int Heu{get{return heuristic;} set{ heuristic = value;}}

        private void Awake() {
            _renderer = GetComponent<SpriteRenderer>();
			//_text = GetComponent<TextMesh>();
            _orig_color = _renderer.color;
            _blocked_color = new Color( _orig_color.r * 0.4f, _orig_color.g * 0.4f, _orig_color.b * 0.8f );
			_checked_color = new Color( _orig_color.r * 0.7f, _orig_color.g * 0.6f, _orig_color.b * 0.46f );
			_wasopen_color = new Color ( _orig_color.r * 1f, _orig_color.g * 0.76f, _orig_color.b * 0.32f );
			_nopath_color = new Color ( _orig_color.r * 1f, _orig_color.g * 0f, _orig_color.b * 0f );
			_path_color = new Color ( _orig_color.r * 1f, _orig_color.g * 1f, _orig_color.b * 0f );
			//this.GetComponentInChildren<MeshRenderer>().sortingLayerName="Icons";
        }

		public void SetText(string texttext){
			//if(isStart == false && isGoal ==false)
			_text.text = texttext;
		}

        public IList<GridNode> GetNeighbors( bool include_diagonal = false ) {
            return grid.GetNodeNeighbors( row, column, include_diagonal );
        }


		private void Update(){
			if (isOnPath) {
				_renderer.color = _path_color;
				_checked = false;
			}

			if (_blocked) {
				_renderer.color = _blocked_color;
				_checked = false;
			}
			if (_checked) {
				_renderer.color = _checked_color;
				if (_wasInOpen) {
					_renderer.color = _wasopen_color;
				}
					
			}

			if (!_blocked && !_checked && !_wasInOpen && !isOnPath)
				_renderer.color = _orig_color;

			if (grid.nopath == true) {
				foreach (GridNode node in Grid.noPathList) {
					node._renderer.color = grid.nopath? _nopath_color:node._renderer.color;
				}			
			}

			if (grid.nopath == false) {
				foreach (GridNode node in Grid.noPathList) {
					node._renderer.color = grid.nopath? _orig_color:node._renderer.color;
				}			
			}
		}
						
    }
}