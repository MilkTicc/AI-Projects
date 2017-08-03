using UnityEngine;
using System.Collections.Generic;


	public class OrientedActor : MonoBehaviour, IActor
	{
		private const float MAX_SPEED = 2.0f;
		private const float STEERING_ACCEL = 60.0f;
		private const float STEERING_LINE_SCALE = 3.0f;
		private const float COLOR_RANGE = 1.0f;
		public Vector2 initialVelocity = Vector2.zero;
		public bool wrapScreen = false;
		public Vector2 position;
		//public LineRenderer _steering_line;
		public Grid grid;
		//public Pathfinding pathFind;
		public SpriteRenderer _renderer;
		//public GridNode currentNode;
		//public UI ui;
		Vector2 _steering = Vector2.zero;
		//Vector2 _acceleration = Vector2.zero;
		Vector2 _velocity = Vector2.zero;

		void Awake ()
		{
			_renderer = GetComponent<SpriteRenderer> ();
		}

		void Start ()
		{
			_velocity = initialVelocity;
		}

        public void SetAcc( float x_axis, float y_axis ) {
            _steering = Vector2.ClampMagnitude( new Vector2(x_axis, y_axis), 30.0f );
            //_acceleration = _steering;
        }

		public void SetVelo( float x_axis, float y_axis ) {
			_velocity = Vector2.ClampMagnitude( new Vector2(x_axis, y_axis), MAX_SPEED );

		}

        public float MaxSpeed {
			get { return MAX_SPEED; }}

        public Vector2 Velocity {
			get { return _velocity; }}
        
		public Vector2 Position {
			get { return position; }}
		
		


        private void Update() {
		_velocity += _steering * Time.deltaTime;
			//_velocity = _velocity.normalized * MAX_SPEED;
            position += (_velocity * Time.deltaTime);
			transform.position +=(Vector3) (_velocity * Time.deltaTime);
		transform.rotation = Quaternion.LookRotation(Vector3.forward , Vector3.Normalize(_velocity));
			//currentNode = grid.PostoNode (transform.position);
			//Debug.Log ("OA" +currentNode);
            //_steering_line.transform.rotation = Quaternion.identity;
            //_steering_line.SetPosition( 1, _steering * STEERING_LINE_SCALE );
            //_steering_line.gameObject.SetActive(true);
            _steering = Vector2.zero;
            //_acceleration = Vector2.zero;
        }
    }
