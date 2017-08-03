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
        public SpriteRenderer _renderer;
		public Sprite openSprite;
		public Sprite crossSprite;
		public Sprite circleSprite;
		public bool scoreChecked = false;
        Color _orig_color;
        [SerializeField]
        private bool _blocked = false;
		private bool _checked = false;
		private bool _wasInOpen = false;

		public enum NodeStat {open, circle, cross}
		NodeStat _nodeStat;

		public NodeStat NodeStatus{ get { return _nodeStat;}set { _nodeStat = value;}}

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

		public void DrawNode(){
			switch(_nodeStat){
				case NodeStat.open:
					_renderer.sprite = openSprite;
					break;
				case NodeStat.cross:
					_renderer.sprite = crossSprite;
					break;
				case NodeStat.circle:
					_renderer.sprite = circleSprite;
					break;
			}
		}

        private void Awake() {
            _renderer = GetComponent<SpriteRenderer>();
            _orig_color = _renderer.color;
        }


        public IList<GridNode> GetNeighbors( int neighbortype) {
			return grid.GetNodeNeighbors( row, column, neighbortype);
        }

		
		private void Update(){
			}
		}
						
}