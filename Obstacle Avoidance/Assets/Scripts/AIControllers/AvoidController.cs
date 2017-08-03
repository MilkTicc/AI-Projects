using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace AISandbox {
    [RequireComponent(typeof (IActor))]
    public class AvoidController : MonoBehaviour {
        private IActor _actor;
		private const float MIN_RSPEED = 15.0f;
		private const float MAX_RSPEED = 20.0f;
		private const float ACCELERATION = 6.0f;
		private const float MAX_STEER = 10.0f;
		private const float MAX_BRAKE = 0.5f;
		private const float COLLISION_LINE_SCALE = 12.0f;
		private float DIR = 1.0f;
		private float BOUND = 15.0f;
		private Vector2 brake = Vector2.zero;
		private Vector2 steer = Vector2.zero;
		private Vector2 acc = Vector2.zero;
		private Vector2 position;
		private float radius;
		//private SpriteRenderer _renderer;
		public LineRenderer _collision_detector1;
		public LineRenderer _collision_detector2;
		public LineRenderer _collision_detector3;
		public RaycastHit2D hit;


		private void Awake() {
            _actor = GetComponent<IActor>();
//			_actor.SetPosition (0.0f, 0.0f);
//			_actor.SetVelocity (0.0f, 20.0f);


			Vector3 velocity = new Vector3(Random.Range(-DIR,DIR), Random.Range(-DIR,DIR ),0.0f) * Random.Range(MIN_RSPEED,MAX_RSPEED );
			_actor.SetVelocity(velocity.x,velocity.y);
			_actor.SetPosition (Random.Range(-BOUND, BOUND), Random.Range(-BOUND, BOUND));

			radius = _actor.Radius ;


        }

		private void ScreenWrap(){
			var cam = Camera.main;
			var viewPosition = cam.WorldToViewportPoint (position);
			//Debug.Log (viewPostion);
			if  (viewPosition.x > 1.04 || viewPosition.x < -0.04) {
				_actor.SetPosition(-position.x, position.y);
			} 
			if (viewPosition.y > 1.04 || viewPosition.y < -0.04) {
				_actor.SetPosition (position.x, -position.y);
			}
				
		}

		private Vector2 Steer(float proportion, bool right){
			int r = right ? 1 : -1;
			return new Vector2 (-_actor.Velocity.y, _actor.Velocity.x).normalized * MAX_STEER  * proportion  * r;
		}

		private Vector2 Brake(float proportion){
			return _actor.Velocity.normalized  * proportion * proportion * proportion * MAX_BRAKE * -1.0f;
		}

		private Vector2 CollisionDetector(){
			
			_collision_detector1.transform.position = _actor.Position + new Vector2 (_actor.Velocity.y, -_actor.Velocity.x ).normalized * radius ;
			_collision_detector1.SetPosition( 1, _actor.Velocity.normalized * COLLISION_LINE_SCALE );
			_collision_detector2.transform.position = _actor.Position + new Vector2 (-_actor.Velocity.y, _actor.Velocity.x ).normalized * radius ;
			_collision_detector2.SetPosition( 1, _actor.Velocity.normalized * COLLISION_LINE_SCALE  );
			_collision_detector3.transform.position = _actor.Position + _actor.Velocity.normalized * COLLISION_LINE_SCALE  - new Vector2 (_actor.Velocity.y, -_actor.Velocity.x ).normalized *radius;
			_collision_detector3.SetPosition( 1, new Vector2 (_actor.Velocity.y, -_actor.Velocity.x ).normalized  * 2.0f *radius );
			//_collision_detector1.gameObject.SetActive (false);
			RaycastHit2D hit1 = Physics2D.Raycast(_collision_detector1.transform.position, _actor.Velocity, COLLISION_LINE_SCALE);
			RaycastHit2D hit2 = Physics2D.Raycast(_collision_detector2.transform.position, _actor.Velocity, COLLISION_LINE_SCALE);
			hit = hit1 ? hit1 : hit2;
			//SpriteRenderer _renderer = hit? hit.transform.GetComponent<SpriteRenderer>(): null;


			//SpriteRenderer _lastrenderer = _renderer;
			if (hit1 || hit2) {
				hit.transform.GetComponent<CircleObstacle> ().detected = true;

				//print (_actor.Velocity.magnitude * COLLISION_LINE_SCALE);
				float dis1 = hit1 ? (hit1.point - new Vector2 (_collision_detector1.transform.position.x, _collision_detector1.transform.position.y)).magnitude : COLLISION_LINE_SCALE;
				float prop1 = (1.0f - dis1 / COLLISION_LINE_SCALE);
				float dis2 = hit2 ? (hit2.point - new Vector2 (_collision_detector2.transform.position.x, _collision_detector2.transform.position.y)).magnitude : COLLISION_LINE_SCALE;
				float prop2 = (1.0f - dis2 / COLLISION_LINE_SCALE);
				float prop = prop1 >= prop2 ? prop1 : prop2;
				bool right = prop1 >= prop2 ? true : false;

				

				//_renderer.color = Color.yellow;

				steer = Steer (prop, right);
				brake = Brake (prop);
				acc = Vector2.zero;

			}
			else {
				steer = Vector2.zero;
				brake= Vector2.zero;


				//_lastrenderer.color = Color.gray;
				//detected = false;
			}
			if (_actor.Velocity.magnitude < (_actor.MaxSpeed - 0.1f) ) {
				//steer = Vector2.zero;
				//brake = Vector2.zero;

				acc = _actor.Velocity.normalized * ACCELERATION;
				//	print(_actor.Velocity.magnitude );

			}
			//_lastrenderer.color = Color.gray;


			//print (_actor.MaxSpeed);
			return steer + brake + acc;

		}

        private void FixedUpdate() {

			position = _actor.Position;
			ScreenWrap();
			Vector2 move = CollisionDetector();

            // Pass all parameters to the character control script.
			_actor.SetInput( move.x, move.y );
			//Debug.Log (_actor.Velocity.magnitude);
        }

	
    }
}