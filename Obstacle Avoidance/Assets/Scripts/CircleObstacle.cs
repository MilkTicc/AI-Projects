using UnityEngine;
using System.Collections;

namespace AISandbox {
    [RequireComponent(typeof (SpriteRenderer))]
    public class CircleObstacle : MonoBehaviour {
        private readonly Color COLLIDE_COLOR = Color.red;
		private readonly Color DETECT_COLOR = Color.yellow;
        private SpriteRenderer _renderer;
        public Color _orig_color;
        private float _radius;
		private float _count;
		private bool collision = false;
		public bool detected = false;


		private void Generate(){
			
		}

        private void Start() {
            _renderer = GetComponent<SpriteRenderer>();
            _orig_color = _renderer.color;
            _radius = _renderer.bounds.extents.x;
        }

		private void DetectionDetector(){
			//GameObject player = GameObject.FindGameObjectWithTag ("Player");
			//RaycastHit2D hit = player.GetComponent<AvoidController> ().hit;
			//SpriteRenderer _rendererdetected =hit ? hit.transform.GetComponent<SpriteRenderer> () : null;

//			if (collision) {
//				_renderer.color = COLLIDE_COLOR;
//			}
////			else if (hit && !collision ) {
//				_rendererdetected.color = DETECT_COLOR ;
//
//			} 
//			else {
//				_renderer.color = _orig_color;
//			}

			if (detected) {
				_renderer.color = DETECT_COLOR;
			}

			detected = false;
		}

        private void FixedUpdate() {
			//detected  = false;
			collision = false;
			_renderer.color = _orig_color;

				SimpleActor[] actors = FindObjectsOfType<SimpleActor>();
            foreach( SimpleActor actor in actors ) {
                float sqr_dist = (actor.transform.position - transform.position).sqrMagnitude;
                float actor_radius = actor.gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;
                float sqr_min_dist = (_radius+actor_radius) * (_radius+actor_radius);
                if( sqr_min_dist > sqr_dist) {
                    collision = true;
                    break;
                }
		
            }

			if (!collision)
				DetectionDetector ();
			else
				_renderer.color = COLLIDE_COLOR;


			//print (detected);
			//_renderer.color = _orig_color;
//			_renderer.color = collision ? COLLIDE_COLOR : _renderer.color;
        }
    }
}
