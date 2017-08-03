using UnityEngine;
using System.Collections;

namespace AISandbox {
    [RequireComponent(typeof (IActor))]
    public class ConstantController : MonoBehaviour {

	
		private const float SPEED_MIN = 10.0f;
		private const float SPEED_MAX = 10.0f;
		private const float DIR = 1.0f;
        private IActor _actor;
		public Vector3 tarPos;
		//private Vector2 velo;
		public Vector3 tarPredPos;

		private void Awake() {
            _actor = GetComponent<IActor>();
			float speed = Random.Range(SPEED_MIN, SPEED_MAX);
			Vector2 tarVelo = new Vector2(Random.Range(-DIR, DIR), Random.Range(-DIR, DIR)).normalized*speed ;
		
			_actor.SetVelocity (tarVelo.x, tarVelo.y);
			//velo = tarVelo;
        }

        private void FixedUpdate() {
            // Pass all parameters to the character control script.
            _actor.SetInput( 0.0f, 0.0f );




        }
    }
}