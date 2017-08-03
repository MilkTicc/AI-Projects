using UnityEngine;
using System.Collections;

namespace AISandbox {
    public class SimpleActor : MonoBehaviour, IActor {
        private const float MAX_SPEED           = 20.0f;
        private const float STEERING_ACCEL      = 100.0f;
        private const float VELOCITY_LINE_SCALE = 0.1f;
        private const float STEERING_LINE_SCALE = 4.0f;
		private const float COLLISION_LINE_SCALE = 0.2f;


        [SerializeField]
        private bool _DrawVectors = true;
        public bool DrawVectors {
            get {
                return _DrawVectors;
            }
            set {
                _DrawVectors = value;
                _steering_line.gameObject.SetActive(_DrawVectors);
				_velocity_line.gameObject.SetActive(_DrawVectors);
				//_collision_detector.gameObject.SetActive(_DrawVectors);

            }
        }
        public LineRenderer _steering_line;
        public LineRenderer _velocity_line;

		public SpriteRenderer _renderer;


        private Vector2 _steering = Vector2.zero;
        private Vector2 _acceleration = Vector2.zero;
        private Vector2 _velocity = Vector2.zero;

        private void Start() {
            DrawVectors = _DrawVectors;
			_renderer = GetComponent<SpriteRenderer>();
        }

        public void SetInput( float x_axis, float y_axis ) {
            _steering = Vector2.ClampMagnitude( new Vector2(x_axis, y_axis), 1.0f );
            _acceleration = _steering * STEERING_ACCEL;
        }

		public void SetVelocity (float x_axis, float y_axis){
			_velocity = Vector2.ClampMagnitude (new Vector2 (x_axis, y_axis), MAX_SPEED);
		}

		public void SetPosition(float x_axis, float y_axis){
			transform.position = new Vector3 (x_axis, y_axis, 0.0f);
		}

        public float MaxSpeed {
            get { return MAX_SPEED; }
        }

		public float Radius {
			get{ return _renderer.bounds.extents.x;}
		}

        public Vector2 Velocity {
            get { return _velocity; }
        }

		public Vector2 Position {
			get { return transform.position; }
		}

        private void FixedUpdate() {
            _velocity += _acceleration * Time.fixedDeltaTime;
            _velocity = Vector2.ClampMagnitude(_velocity, MAX_SPEED);
            Vector3 position = transform.position;
            position += (Vector3)(_velocity * Time.fixedDeltaTime);
            transform.position = position;
        }

        private void Update() {
            _steering_line.SetPosition( 1, _steering * STEERING_LINE_SCALE );
            _velocity_line.SetPosition( 1, _velocity * VELOCITY_LINE_SCALE );

            // The steering is reset every frame so SetInput() must be called every frame for continuous steering.
            _steering = Vector2.zero;
            _acceleration = Vector2.zero;
        }
    }
}