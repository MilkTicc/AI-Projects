using UnityEngine;
using System.Collections;

namespace AISandbox {
    public class SimpleActor : MonoBehaviour, IActor {
        private const float MAX_SPEED           = 40.0f;
        private const float STEERING_ACCEL      = 100.0f;
        private const float VELOCITY_LINE_SCALE = 0.1f;
        private const float STEERING_LINE_SCALE = 4.0f;

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
            }
        }
        public LineRenderer _steering_line;
        public LineRenderer _velocity_line;

        private Vector2 _steering = Vector2.zero;
        private Vector2 _acceleration = Vector2.zero;
		private Vector2 _velocity = Vector2.zero;
	

        private void Start() {
            DrawVectors = _DrawVectors;
        }

		public void SetInput( float x_axis, float y_axis) {
            _steering = Vector2.ClampMagnitude( new Vector2(x_axis, y_axis), 1.0f );
            _acceleration = _steering * STEERING_ACCEL;
        }


		public Vector2 GetVelocity ()
		{
			return new Vector2 (_velocity.x, _velocity.y); 
		}

		public void SetVelocity (float i_x, float i_y){
			_velocity = Vector2.ClampMagnitude (new Vector2 (i_x, i_y), MAX_SPEED);
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
        }
    }
}