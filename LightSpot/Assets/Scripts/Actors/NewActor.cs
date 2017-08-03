using System.Collections;
using System.Collections.Generic;
using UnityEngine;


	public class NewActor : MonoBehaviour,IActor {
		private const float MAX_SPEED = 2f;
		private const float STEERING_ACC = 50f;
		private const float COLOR_RANGE = 0.9f;
		public Vector2 initialVelo = Vector2.zero;
		//private SpriteRenderer _renderer;
		private Vector2 _steering = Vector2.zero;
		private Vector2 _velo = Vector2.zero;
		private Vector2 _acceleration = Vector2.zero;
		private Vector2 _position;
		public GridNode currentNode;
		public Grid grid;


		private void Start () {
			_velo = initialVelo;

		}

		public float MaxSpeed{ get { return MAX_SPEED; } }

		public void SetAcc(float x, float y){
			_steering = Vector2.ClampMagnitude (new Vector2 (x, y), 50f);
			_acceleration = _steering;
		}

		public void SetVelo(float x, float y){
			_velo = new Vector2 (x, y).normalized * MAX_SPEED;
			//_velo = Vector2.ClampMagnitude (new Vector2 (x, y), MAX_SPEED);
		}

		public Vector2 Velocity{ get { return _velo; } set { _velo = value; } }
		public Vector2 Position{ get { return _position ; } set { _position = value; } }
		// Update is called once per frame
		void Update () {
			_velo += _acceleration * Time.deltaTime;
			_position += _velo * Time.deltaTime;
			transform.position += (Vector3)_velo * Time.deltaTime;
			//transform.position = _position;
			transform.rotation = Quaternion.LookRotation (Vector3.back, Vector3.Normalize (_velo));
			currentNode = grid.PostoNode (transform.position);
		//currentNode._renderer.color = Color.green;


			//Debug.Log (currentNode);
			_steering = Vector2.zero;
			_acceleration = Vector2.zero;
		}
	}
