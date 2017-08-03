using UnityEngine;
using System.Collections;

namespace AISandbox {
    [RequireComponent(typeof (IActor))]
    public class EvadeController : MonoBehaviour {

		private const float SPEED_MIN = 10.0f;
		private const float SPEED_MAX = 10.0f;
		private const float DIR = 1.0f;
		private const float MAX_SPEED = 40.0f;

        private IActor _actor;
		private Vector3 tarPredPos;
		public GameObject Target;
		private Vector2 direction;
		private Vector3 steer;



		private void Awake() {
            _actor = GetComponent<IActor>();
			float speed = Random.Range(SPEED_MIN, SPEED_MAX);
			Vector2 tarVelo = new Vector2(Random.Range(-DIR, DIR), Random.Range(-DIR, DIR)).normalized*speed ;

			_actor.SetVelocity (tarVelo.x, tarVelo.y);
        }

        private void FixedUpdate() {
			Vector2 purPos = GetComponent<Transform>().position ;
			Vector2 tarPos = Target.transform.position;
			float distance = (tarPos - purPos).magnitude;
			float T = distance / MAX_SPEED;

			Vector2 tarPredPos = tarPos + Target.GetComponent<IActor> ().Velocity * T;

			//Debug.Log(Target.GetComponent<IActor> ().GetVelocity ());

			direction = (tarPredPos - purPos).normalized;

			steer = -(direction * MAX_SPEED - _actor.Velocity);

            // Pass all parameters to the character control script.
			_actor.SetInput( steer.x , steer.y );
        }
    }
}